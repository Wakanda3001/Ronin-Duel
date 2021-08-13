using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimberPowerupTrigger : MonoBehaviour
{
    public IndexUnityEvent OnTriggerActivated;
    public ClimberGameController gameController;

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
        if((playerNum == PlayerIndex.One && transform.position.x > gameController.midline) || (playerNum == PlayerIndex.Two && transform.position.x < gameController.midline)) { return; }

        OnTriggerActivated?.Invoke(playerNum);
        Destroy(gameObject);
    }
}
