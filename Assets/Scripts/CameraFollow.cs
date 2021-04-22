using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;
    public Camera cam;

    private float horzDistance;
    private float vertDistance;

    public float minSize = 5f;

    //Smoothing on camera movement and camera scaling. Closer to 0 means more smoothing, closer to 1 means less.
    public float smoothing = 0.01f;
    public float zoomSmoothing = 0.01f;

    // Called in LateUpdate because needs to go after movement and other stuff
    void LateUpdate()
    {
        //Moves camera to midpoint between players, applies smoothing.
        Vector3 midpoint = new Vector3((player1.transform.position.x + player2.transform.position.x) / 2, (player1.transform.position.y + player2.transform.position.y) / 2, -10);
        Vector3 smoothedMidpoint = Vector3.Lerp(transform.position, midpoint, smoothing);
        transform.position = smoothedMidpoint;

        horzDistance = Math.Abs(player1.transform.position.x - player2.transform.position.x);
        vertDistance = Math.Abs(player1.transform.position.y - player2.transform.position.y);

        float desiredHorz = horzDistance * ((float)Screen.height / (float)Screen.width); //desiredHorz multiplies distance between players by aspect ratio to get camera size nessesary to fit both players.

        float smoothedHorz = Mathf.Lerp(cam.orthographicSize, desiredHorz, zoomSmoothing);
        float smoothedVert = Mathf.Lerp(cam.orthographicSize, vertDistance, zoomSmoothing);

        if (smoothedHorz > minSize)
        {
            cam.orthographicSize = smoothedHorz;
        }
        if (cam.orthographicSize < vertDistance)
        {
            cam.orthographicSize = smoothedVert;
        }
    }
}