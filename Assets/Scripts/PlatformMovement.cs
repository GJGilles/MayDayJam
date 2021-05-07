using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float m_LoopTime = 2;
    private float m_CurrentPos = 0;
    public Transform m_PointA;
    public Transform m_PointB;

    private int m_Forwards = 1;
    private List<GameObject> touching = new List<GameObject>();

    private void Update()
    {
        Vector2 old = transform.position;
        Vector2 pos = Vector2.Lerp(m_PointA.position, m_PointB.position, m_CurrentPos);

        foreach(var obj in touching)
        {
            var rb = obj.GetComponent<Rigidbody2D>();
            rb.position = rb.position + pos - old;
        }

        transform.position = pos;

        if (m_CurrentPos <= 0)
        {
            m_Forwards = 1;
            m_CurrentPos = 0;
        }

        if (m_CurrentPos >= 1)
        {
            m_Forwards = -1;
            m_CurrentPos = 1;
        }

        m_CurrentPos += m_Forwards * 2 * Time.deltaTime / m_LoopTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        touching.Add(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        touching.Remove(collision.gameObject);

        var rb = GetComponent<Rigidbody2D>();
        var other = collision.gameObject.GetComponent<Rigidbody2D>();
        //other.velocity = new Vector2(other.velocity.x - rb.velocity.x, other.velocity.y - rb.velocity.y);
    }
}
