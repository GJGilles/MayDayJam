using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpeedContent : MonoBehaviour
{

    public Vector2 force;

    private GameObject other = null;


    public void Start()
    {
        if (force.x > 0)
        {
            transform.Rotate(0, 0, -90);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        other = collision.gameObject;

        GetComponent<AudioSource>().Play();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        other = null;
    }

    private void FixedUpdate()
    {
        var collider = GetComponent<Collider2D>();
        
        if (other != null && collider.IsTouching(other.GetComponent<Collider2D>()))
        {
            other.GetComponent<Rigidbody2D>().AddForce(force);
        }
    }
}
