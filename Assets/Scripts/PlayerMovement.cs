using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController character;
    public float runSpeed = 40f;

    public string _playerIdentifier = "P1";

    private float _horizontalMove = 0f;
    private bool _jump = false;

    // Update is called once per frame, and is used for input and rendering
    void Update()
    {
        _horizontalMove = Input.GetAxisRaw("Horizontal_" + _playerIdentifier) * runSpeed; // Store input from player controllers

        if (Input.GetButtonDown("Jump_" + _playerIdentifier))
        {
            _jump = true;
        }
    }

    // FixedUpdate is called every .02 seconds (~50 fps), and is used for physics
    void FixedUpdate()
    {
        character.Move(_horizontalMove * Time.fixedDeltaTime, _jump); // Send move commands to player character based on stored input
        _jump = false;
    }
}
