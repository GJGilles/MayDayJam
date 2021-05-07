using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnTouch : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(null);

        var rb = GetComponent<Rigidbody2D>();
        var other = collision.gameObject.GetComponent<Rigidbody2D>();
        other.velocity = new Vector2(other.velocity.x - rb.velocity.x, other.velocity.y - rb.velocity.y);
    }
}
