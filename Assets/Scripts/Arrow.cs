using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject switcher;
    public Camera cam;
    public GameObject arrow;
    public EdgeCollider2D left;
    public EdgeCollider2D top;
    public EdgeCollider2D right;
    public EdgeCollider2D bottom;

    public LayerMask correctLayer;

    void Update()
    {  
        Vector2 upperLeft = new Vector2(0, Screen.height);
        Vector2 upperRight = new Vector2(Screen.width, Screen.height);
        Vector2 lowerLeft = new Vector2(0, 0);
        Vector2 lowerRight = new Vector2(Screen.width, 0);

        upperLeft = Camera.main.ScreenToWorldPoint(upperLeft);
        upperRight = Camera.main.ScreenToWorldPoint(upperRight);
        lowerLeft = Camera.main.ScreenToWorldPoint(lowerLeft);
        lowerRight = Camera.main.ScreenToWorldPoint(lowerRight);

        left.SetPoints(new List<Vector2> { upperLeft, lowerLeft });
        right.SetPoints(new List<Vector2> { upperRight, lowerRight });
        top.SetPoints(new List<Vector2> { upperLeft, upperRight });
        bottom.SetPoints(new List<Vector2> { lowerLeft, lowerRight});


        RaycastHit2D hit = Physics2D.Linecast(cam.transform.position, switcher.transform.position, correctLayer);
        if (!hit)
        {
            arrow.SetActive(false);
        } else
        {
            arrow.transform.position = hit.point;
            arrow.SetActive(true);
        }
    }
}
