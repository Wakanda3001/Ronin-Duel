using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSlide : MonoBehaviour
{
    public PlayerIndex player;

    public Slider hueSlider;

    private void Start()
    {
        if(player == PlayerIndex.One)
        {
            float H, S, V;
            Color.RGBToHSV(ColorManager.player1Color, out H, out S, out V);
            hueSlider.value = H;
        }
        else
        {
            float H, S, V;
            Color.RGBToHSV(ColorManager.player2Color, out H, out S, out V);
            hueSlider.value = H;
        }
    }
    private void Update()
    {
        if(player == PlayerIndex.One)
        {
            ColorManager.ChangeColor(PlayerIndex.One, Color.HSVToRGB(hueSlider.value, 1, 1));
            ColorManager.ChangeBackground(PlayerIndex.One, Color.HSVToRGB(hueSlider.value, 0.5f, 0.25f));
        }
        else
        {
            ColorManager.ChangeColor(PlayerIndex.Two, Color.HSVToRGB(hueSlider.value, 1, 1));
            ColorManager.ChangeBackground(PlayerIndex.Two, Color.HSVToRGB(hueSlider.value, 0.5f, 0.25f));
        }
    }
}
