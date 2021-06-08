using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerReporter : MonoBehaviour
{
    public UnityEvent OnTriggerActivated;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.transform.parent.GetComponent<CharacterController>())
        {
            OnTriggerActivated?.Invoke();
        }
    }
}
