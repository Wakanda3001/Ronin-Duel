using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{

    [SerializeField]
    private AnimationCurve hueToValue;

    public static ColorManager instance {get; private set;}
    public static Color player1Default = new Color(26f/255f, 156f/255f, 217f/255f, 1);
    public static Color player2Default = new Color(255f/255f, 90f/255f, 64f/255f, 1);

    public static Color player1Color { get; private set; }
    public static Color player2Color { get; private set; }
    public static Color player1Background = new Color(32f / 255f, 54f / 255f, 64f / 255f, 1);
    public static Color player2Background = new Color(64 / 255f, 36 / 255f, 32 / 255f, 1);

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        ChangeColor(PlayerIndex.One, player1Default);
        ChangeColor(PlayerIndex.Two, player2Default);
        ChangeBackground(PlayerIndex.One, player1Background);
        ChangeBackground(PlayerIndex.Two, player2Background);
    }

    public static void ChangeColor(PlayerIndex player, Color color)
    {
        if(player == PlayerIndex.One)
        {
            player1Color = color;
        }
        else if(player == PlayerIndex.Two)
        {
            player2Color = color;
        }
    }
    public static void ChangeBackground(PlayerIndex player, Color color)
    {
        if (player == PlayerIndex.One)
        {
            player1Background = color;
        }
        else if (player == PlayerIndex.Two)
        {
            player2Background = color;
        }
    }

    public void ResetColors()
    {
        player1Color = player1Default;
        player2Color = player2Default;
        float H, S, V;
        Color.RGBToHSV(player1Default, out H, out S, out V);
        player1Background = Color.HSVToRGB(H, hueToValue.Evaluate(H), 0.25f);
        Color.RGBToHSV(player2Default, out H, out S, out V);
        player2Background = Color.HSVToRGB(H, hueToValue.Evaluate(H), 0.25f);
    }
}
