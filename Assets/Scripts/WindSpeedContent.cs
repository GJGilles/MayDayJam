using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindSpeedContent : MonoBehaviour
{

    public float force = 1;

    private void OnCollisionStay2D(Collision2D collision)
    {
        Rigidbody2D rigid = collision.gameObject.GetComponent<Rigidbody2D>();
        rigid.AddForce(new Vector2(0, force));
    }
}
