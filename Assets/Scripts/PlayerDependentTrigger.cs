using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IndexUnityEvent : UnityEvent<PlayerIndex> { }
public class PlayerDependentTrigger : MonoBehaviour
{
    public IndexUnityEvent OnTriggerActivated;
    public GameController gameController;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<CharacterController>())
        {
            return;
        }
        CharacterController controller = collision.gameObject.GetComponent<CharacterController>();
        PlayerIndex playerNum = PlayerIndex.One;
        if (controller == gameController._playerOne)
        {
            playerNum = PlayerIndex.One;
        }
        else if (controller == gameController._playerTwo)
        {
            playerNum = PlayerIndex.Two;
        }
        else
        {
            return;
        }

        OnTriggerActivated?.Invoke(playerNum);
    }
}
