using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapSceneUI : MonoBehaviour
{
    public Object scene;

    public void OnClick()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(scene.name);
    }
}
