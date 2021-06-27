using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public Object scene;
    [SerializeField]
    private bool doResize;
    [SerializeField]
    private float resizeAmount = 0.1f;

    private void OnMouseEnter()
    {
        if (doResize)
        {
            transform.localScale = new Vector3(transform.localScale.x * 1f + resizeAmount, transform.localScale.y * 1f + resizeAmount, transform.localScale.z * 1f + resizeAmount); //scales up by resizeAmount
        }
    }
    private void OnMouseExit()
    {
        if (doResize)
        {
            transform.localScale = new Vector3(transform.localScale.x * 1f - resizeAmount, transform.localScale.y * 1f - resizeAmount, transform.localScale.z * 1f - resizeAmount);
        }
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene(scene.name);
    }
}
