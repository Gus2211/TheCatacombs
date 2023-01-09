using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wall Variables", menuName = "WallData")]
public class WallData : ScriptableObject
{
    [Header("WallRun")]
    public LayerMask WhatIsWall;
    public float wallRunForce;

    public float maxWallRunTime;
    public float WallRunTimer;
    public float wallCheckDistance;
    public float minJumpHeight;
    public bool wallRight;
    public bool wallLeft;
    public float wallRunSpeed;
    public bool wallrunning;
    public float dtime;
    public float sdtime;

    [Space(40)]
    [Header("WallJump")]
    public float wallJumpForce;
    public float wallJumpSideForce;
    public bool exitingWall;
    public float exitWallTime;
    public float exitWallTimer;
}
