using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMenu : MonoBehaviour
{
    public GameObject desiredMenu;
    public GameObject prevMenu;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GameObject.Find("AudioSource").GetComponent<AudioSource>();
    }
    private void OnMouseDown()
    {
        prevMenu.SetActive(false);
        desiredMenu.SetActive(true);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
