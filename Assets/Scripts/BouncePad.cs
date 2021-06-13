using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float power = 30f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponent<Rigidbody2D>() || !collision.gameObject.GetComponent<CharacterController>())
        {
            return;
        }
        Rigidbody2D rigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
        CharacterController character = collision.gameObject.GetComponent<CharacterController>();

        //rigidBody.AddForce(transform.up * power);
        character.bounceVelocity = transform.up * power;
        character.bouncing = true;
        
    }
}
