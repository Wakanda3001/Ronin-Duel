using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorNumbers : MonoBehaviour
{
    public TMP_InputField red;
    public TMP_InputField green;
    public TMP_InputField blue;

    public PlayerIndex player;

     void Start()
     {
        if (player == PlayerIndex.One)
        {
            red.text = (ColorManager.player1Default.r * 255).ToString();
            green.text = (ColorManager.player1Default.g * 255).ToString();
            blue.text = (ColorManager.player1Default.b * 255).ToString();
        }
        else
        {
            red.text = (ColorManager.player2Default.r * 255).ToString();
            green.text = (ColorManager.player2Default.g * 255).ToString();
            blue.text = (ColorManager.player2Default.b * 255).ToString();
        }
     }

    private void Update()
    {
        int redI = 0;
        int greenI = 0;
        int blueI = 0;

        int.TryParse(red.text, out redI);
        int.TryParse(blue.text, out blueI);
        int.TryParse(green.text, out greenI);

        if (player == PlayerIndex.One)
        {
            ColorManager.ChangeColor(PlayerIndex.One, new Color(redI / 255f, greenI / 255f, blueI / 255f, 255));
        }
        else
        {
            ColorManager.ChangeColor(PlayerIndex.Two, new Color(redI / 255f, greenI / 255f, blueI / 255f, 255));
        }
    }
}
