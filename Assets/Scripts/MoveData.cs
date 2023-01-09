using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Variables", menuName = "MoveData")]
public class MoveData : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float mouseSpeed;
    public bool running;
    public bool walking;

    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;

    public Vector2 turn = new Vector2(0f, 0f);
    public float maxcamDown = -17;
    public float maxcamUp = 60;


    [Space(40)]
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Space(40)]
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGroundL;

    public float GroundCheckDistance;
    
    public bool grounded;
    public string whatIsGround;



    public float horizontalInput;
    public float verticalInput;

    public Vector3 moveDirection;

    [Space(40)]
    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;
}
