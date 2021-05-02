using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;

    public float runSpeed = 40f;

    private float horzMove = 0f;
    private bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horzMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
    }

    // Update is called a fixed number of times per second
    private void FixedUpdate()
    {
        controller.Move(horzMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
