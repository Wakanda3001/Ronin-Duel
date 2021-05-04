using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GOUnityEvent : UnityEvent<GameObject> { }
public class PlayerDependentTrigger : MonoBehaviour
{
    public GOUnityEvent OnTriggerActivated;

    void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerActivated?.Invoke(collision.gameObject);
    }
}
