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
            ColorManager.player1Color = Color.HSVToRGB(hueSlider.value, 1, 1);
            ColorManager.player1Background = Color.HSVToRGB(hueSlider.value, 0.5f, 0.25f);
        }
        else
        {
            ColorManager.player2Color = Color.HSVToRGB(hueSlider.value, 1, 1);
            ColorManager.player2Background = Color.HSVToRGB(hueSlider.value, 0.5f, 0.25f);
        }
    }
}
