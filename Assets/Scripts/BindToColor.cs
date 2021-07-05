using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BindToColor : MonoBehaviour
{
    public bool isPlayer1;

    private void Update()
    {
        if (isPlayer1)
        {
            if (GetComponent<SpriteRenderer>())
            {
                GetComponent<SpriteRenderer>().color = ColorManager.player1Color;
            }
            else if (GetComponent<Image>())
            {
                GetComponent<Image>().color = ColorManager.player1Color;
            }
        }
        else
        {
            if (GetComponent<SpriteRenderer>())
            {
                GetComponent<SpriteRenderer>().color = ColorManager.player2Color;
            }
            else if (GetComponent<Image>())
            {
                GetComponent<Image>().color = ColorManager.player2Color;
            }
        }
    }
}
