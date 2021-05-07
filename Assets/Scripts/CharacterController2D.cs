using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_MoveSpeed = 400f;
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[SerializeField] private float m_JumpCut = 0.8f;
	[SerializeField] private float m_JumpControlTime = 0.6f;
	[SerializeField] private float m_DashForce = 400f;                          // Amount of force added when the player dashes.
	[SerializeField] private float m_DashCooldown = 1f;
	[SerializeField] private float m_DashControlTime = 1f;
	[SerializeField] private float m_DashSmoothing = 0.6f;
	[SerializeField] private float m_SpeedSmoothing = 0.2f;
	[SerializeField] private float m_SlowSmoothing = 0.1f;
	[SerializeField] private float m_StopSmoothing = 0.05f;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	const float k_JumpInputTime = 0.3f;
	const float k_GroundedInputTime = 0.3f;
	private float m_JumpInputLast = 0f;
	private float m_GroundedInputLast = 0f;
	private float m_JumpTimeLast = 0f;
	private float m_DashTimeLast = 0f;


	const string MOVE_PARAM = "Moving";
	const string JUMP_PARAM = "Jumping";
	const string DASH_PARAM = "Dashing";

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}


	// Called on fixed update by the input manager
	public void Move(float move, bool jump, bool dash)
	{
		var animator = GetComponent<Animator>();

        #region Grounded Check
        bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		m_GroundedInputLast -= Time.deltaTime;
		if (m_Grounded)
        {
			m_GroundedInputLast = k_GroundedInputTime;
        }
		#endregion

		#region Jump Cutting

		// If Jump Ended 
		if (m_JumpTimeLast <= m_JumpControlTime && (m_JumpTimeLast + Time.deltaTime) > m_JumpControlTime)
		{
			if (m_Rigidbody2D.velocity.y > 0)
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, m_Rigidbody2D.velocity.y * m_JumpCut);
			animator.SetBool(JUMP_PARAM, false);
		}

		if (m_JumpTimeLast <= m_JumpControlTime)
		{
			m_JumpTimeLast += Time.deltaTime;

			if (!jump && m_JumpTimeLast < m_JumpControlTime)
				m_JumpTimeLast = m_JumpControlTime;

		}
		#endregion

		#region Jump Debouncing
		m_JumpInputLast -= Time.deltaTime;
		if (jump)
        {
			m_JumpInputLast = k_JumpInputTime;
        }

		jump = jump && (m_GroundedInputLast > 0) && (m_JumpInputLast > 0);
		if (jump)
        {
			m_GroundedInputLast = 0;
			m_JumpInputLast = 0;
        }
		#endregion

		#region Flip Facing
		// If the input is moving the player right and the player is facing left...
		if (move > 0 && !m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (move < 0 && m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
		#endregion

		#region Ground Movement
		float newMove = move * m_MoveSpeed * Time.deltaTime;
		float currMove = m_Rigidbody2D.velocity.x;
		float threshold = 0.001f;

		// Dashing
		if (m_DashTimeLast <= m_DashControlTime)
        {
			Vector3 targetVelocity = new Vector2(0, 0);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_DashSmoothing);
		}
		// Speeding up
		else if (((newMove >= -1*threshold && currMove >= -1*threshold) || (newMove <= threshold && currMove <= threshold)) && Mathf.Abs(newMove) >= Mathf.Abs(currMove))
        {
			Vector3 targetVelocity = new Vector2(newMove, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_SpeedSmoothing);
		}
		// Slowing down (Positive)
		else if (currMove >= threshold && currMove >= 1 * m_MoveSpeed * Time.deltaTime)
		{
			Vector3 targetVelocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_SlowSmoothing);
		}
		// Slowing Down (Negative)
		else if (currMove <= -1*threshold && currMove <= -1 * m_MoveSpeed * Time.deltaTime)
		{
			Vector3 targetVelocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_SlowSmoothing);
		}
		// Stopping 
        else
        {
			Vector3 targetVelocity = new Vector2(0, m_Rigidbody2D.velocity.y);
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_StopSmoothing);
		}

		animator.SetBool(MOVE_PARAM, (Mathf.Abs(m_Rigidbody2D.velocity.x) > threshold));

		#endregion

		#region Action Movement
		// If the player should jump...
		if (jump)
		{
			// Add a vertical force to the player.
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			m_JumpTimeLast = 0;
			animator.SetBool(JUMP_PARAM, true);
		}

		// If the dash has ended
		if (m_DashTimeLast <= m_DashControlTime && (m_DashTimeLast + Time.deltaTime) > m_DashControlTime)
        {
			m_Rigidbody2D.velocity = new Vector2(0, 0);
			animator.SetBool(DASH_PARAM, false);
        }

		// If the player should dash
		if (dash && m_DashTimeLast > m_DashCooldown)
        {
			float force = m_DashForce * (m_FacingRight ? 1 : -1);
			m_Rigidbody2D.AddForce(new Vector2(force, 0f));
			if (m_Rigidbody2D.velocity.y <= threshold)
            {
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0f);
            }
			m_DashTimeLast = 0;
			animator.SetBool(DASH_PARAM, true);
		}

		if (m_DashTimeLast <= m_DashCooldown)
			m_DashTimeLast += Time.deltaTime;

		#endregion
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
