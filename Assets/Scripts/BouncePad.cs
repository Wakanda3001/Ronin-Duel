using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float power = 1000f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<Rigidbody2D>())
        {
            return;
        }
        Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();

        rigidBody.velocity = transform.up * power;
    }
}
