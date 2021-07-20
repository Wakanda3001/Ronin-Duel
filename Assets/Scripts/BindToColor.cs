using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BindToColor : MonoBehaviour
{
    public bool isPlayer1;
    public bool isBackground = false;

    private void Start()
    {
        if (ColorManager.player1Color.a == 0f || ColorManager.player2Color.a == 0f)
        {
            ColorManager.instance.ResetColors();
        }
    }

    private void ChangeColor(Color color)
    {
        if (GetComponent<SpriteRenderer>())
        {
            GetComponent<SpriteRenderer>().color = color;
        }
        else if (GetComponent<Image>())
        {
            GetComponent<Image>().color = color;
        }
        else if (GetComponent<TextMeshPro>())
        {
            GetComponent<TextMeshPro>().color = color;
        }
    }

    private void Update()
    {
        if (!isBackground)
        {
            if (isPlayer1)
            {
                ChangeColor(ColorManager.player1Color);
            }
            else
            {
                ChangeColor(ColorManager.player2Color);
            }
        }
        else
        {
            if (isPlayer1)
            {
                ChangeColor(ColorManager.player1Background);
            }
            else
            {
                ChangeColor(ColorManager.player2Background);
            }
        }
    }
}
