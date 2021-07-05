using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorManager : MonoBehaviour
{
    public TMP_InputField red;
    public TMP_InputField blue;
    public TMP_InputField green;
    [SerializeField]
    int redI = 0;
    [SerializeField]
    int blueI = 0;
    [SerializeField]
    int greenI = 0;

    public static Color player1Color;
    public static Color player2Color;

    float H;
    float S;
    float V;

    public PlayerIndex player;

    private void Update()
    {
        int.TryParse(red.text, out redI);
        int.TryParse(blue.text, out blueI);
        int.TryParse(green.text, out greenI);

        if(player == PlayerIndex.One)
        {
            //Color.RGBToHSV(player1Color, out H, out S, out V); use hsv for better control over colors
            player1Color = new Color32((byte)redI, (byte)blueI, (byte)greenI, 255);
        }
        else
        {
            player2Color = new Color32((byte)redI, (byte)blueI, (byte)greenI, 255);
        }
    }
}
