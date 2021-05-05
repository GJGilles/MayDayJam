using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;

    private float horzMove = 0f;
    private bool jump = false;
    private bool dash = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horzMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetKey(KeyCode.Space))
        {
            jump = true;
        }
        else
        {
            jump = false;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            dash = true;
        }
        else
        {
            dash = false;
        }
    }

    // Update is called a fixed number of times per second
    private void FixedUpdate()
    {
        controller.Move(horzMove, jump, dash);
    }
}
