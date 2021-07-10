using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ClimberCamera : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public Camera cam;

    private float vertDistance;

    public float minSize = 5f;
    public float maxsize = 25f;

    public Vector3 offset = new Vector3(0f,2f,0f);

    //Smoothing on camera movement and camera scaling. Closer to 0 means more smoothing, closer to 1 means less.
    public float smoothing = 1f;
    public float zoomSmoothing = 1f;

    // Called in LateUpdate because needs to go after movement and other stuff
    void LateUpdate()
    {
        //Moves camera to midpoint between players, applies smoothing.
        Vector3 midpoint = new Vector3((player1.position.x + player2.position.x) / 2, (player1.position.y + player2.position.y) / 2, -10);
        Vector3 smoothedMidpoint = Vector3.Lerp(cam.transform.position-offset, midpoint, smoothing * Time.deltaTime);
        cam.transform.position = smoothedMidpoint+offset;

        vertDistance = Math.Abs(player1.position.y - player2.transform.position.y);
        float smoothedVert = Mathf.Lerp(cam.orthographicSize, vertDistance, zoomSmoothing * Time.deltaTime);

        if (smoothedVert > minSize && smoothedVert < maxsize)
        {
            cam.orthographicSize = smoothedVert;
        }
        
    }
}
