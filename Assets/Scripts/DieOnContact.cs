using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieOnContact : MonoBehaviour
{
    public GameController gameController;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<CharacterController>())
        {
            CharacterController controller = collision.gameObject.GetComponent<CharacterController>();
            if (controller == gameController._playerOne)
            {
                gameController.KillPlayer(PlayerIndex.One);
            }
            else if (controller == gameController._playerTwo)
            {
                gameController.KillPlayer(PlayerIndex.Two);
            }
        }
    }
}

