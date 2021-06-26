using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    public Collider2D platform;
    private void OnTriggerEnter2D(Collider2D collision)
    {
       Physics2D.IgnoreCollision(collision, platform, true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Physics2D.IgnoreCollision(collision, platform, false);
    }
}
