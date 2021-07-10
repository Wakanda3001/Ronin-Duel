using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColorManager : MonoBehaviour
{
    public static Color player1Default = new Color(26f/255f, 156f/255f, 217f/255f, 1);
    public static Color player2Default = new Color(255f/255f, 90f/255f, 64f/255f, 1);

    public static Color player1Color = player1Default;
    public static Color player2Color = player2Default;
    public static Color player1Background = new Color(32f / 255f, 54f / 255f, 64f / 255f, 1);
    public static Color player2Background = new Color(64 / 255f, 36 / 255f, 32 / 255f, 1);

    public static void ResetColors()
    {
        player1Color = player1Default;
        player2Color = player2Default;
        float H, S, V;
        Color.RGBToHSV(player1Default, out H, out S, out V);
        player1Background = Color.HSVToRGB(H, 0.5f, 0.25f);
        Color.RGBToHSV(player2Default, out H, out S, out V);
        player2Background = Color.HSVToRGB(H, 0.5f, 0.25f);
    }
}
