using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMenu : MonoBehaviour
{
    public GameObject desiredMenu;
    public GameObject prevMenu;
    private void OnMouseDown()
    {
        prevMenu.SetActive(false);
        desiredMenu.SetActive(true);
    }
}
