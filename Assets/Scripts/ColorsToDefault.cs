using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorsToDefault : MonoBehaviour
{
    private void OnMouseDown()
    {
        ColorManager.ResetColors();
    }
}
