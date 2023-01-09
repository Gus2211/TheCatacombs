using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    private bool mFaded = false;
    public float Duration = 0.4f;


    public GameObject TutorialCanvas;
    public AudioSource FSound;
    public GameObject pSpawn;
    public GameObject GameOverCanvas;
    public bool MouseMove;

    public Transform GroundCheck;
    public Transform orientation;
    public Rigidbody rb;

    public GameObject CameraPivot;
    public MoveData data;
    public WallData wall;

    private RaycastHit rightWallHit;
    private RaycastHit leftWallHit;
    
    public MovementState state;

    public WallRunCam WallCam;
    public enum MovementState
    {
        walking,
        running,
        wallrunning
    }

    private void StateHandler()
    {
        if(wall.wallrunning)
        {
            state = MovementState.wallrunning;
            data.moveSpeed = wall.wallRunSpeed;
        }
        
        if(data.walking)
        {
            state = MovementState.walking;
            data.moveSpeed = data.walkSpeed;
        }

        if (data.running)
        {
            state = MovementState.running;
            data.moveSpeed = data.runSpeed;
        }

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        data.readyToJump = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        MouseMove = false;


        
    }
    
    private void Update()
    {
        // ground check
        //grounded = Physics.Raycast(transform.position, Vector3.down, GroundCheckDistance, whatIsGround);
        Debug.DrawLine(GroundCheck.position, GroundCheck.position + new Vector3(0f, -data.GroundCheckDistance, 0f), Color.red);

        RaycastHit hit;
        if (Physics.Raycast(GroundCheck.position, Vector3.down, out hit, data.GroundCheckDistance))
        {
            if (hit.collider.tag == data.whatIsGround)
            {
                data.grounded = true;
            }
            else
            {
                data.grounded = false;
            }
        }
        else
        {
            data.grounded = false;
        }

        CheckForWall();
        MyInput();
        if (MouseMove == false)
        {

        }
        else
        {
            MouseMovement();
        }
        SpeedControl();
        StateMachine();
        StateHandler();
        
      
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Void")
        {
            GameOverCanvas.SetActive(true);
            var CanvasGroup  = GameOverCanvas.GetComponent<CanvasGroup>();
            CanvasGroup.alpha = 0f;
            Fade();
            this.transform.position = pSpawn.transform.position;
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
            CameraPivot.transform.rotation = new Quaternion(0, 0, 0, 0);
            
            TutorialCanvas.SetActive(false);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            MouseMove = false;       
        }
    }
    public void Fade()
    {
        var CanvGroup = GameOverCanvas.GetComponent<CanvasGroup>();
        StartCoroutine(DoFade(CanvGroup, CanvGroup.alpha, mFaded ? 1:0));
    }

    public IEnumerator DoFade(CanvasGroup CanvGroup, float start, float end)
    {
        float counter = 0f;
        float cd = 1f;

        while(Time.time > counter && CanvGroup.alpha != 1)
        {
            counter = Time.deltaTime + cd;
            CanvGroup.alpha = Mathf.Lerp(start, 1, counter / Duration);
            Debug.Log(CanvGroup.alpha);
            
        }
        yield return null;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Void")
        {
            this.transform.position = pSpawn.transform.position;
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
            CameraPivot.transform.rotation = new Quaternion(0, 0, 0, 0);
            GameOverCanvas.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            MouseMove = false;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
        Run();
        if(wall.wallrunning)
        {
            WallRunMovement();
        }
        if (data.grounded)
            rb.drag = data.groundDrag;
        else
            rb.drag = 0;
    }

    private void MyInput()
    {
        data.horizontalInput = Input.GetAxisRaw("Horizontal");
        data.verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(data.jumpKey) && data.readyToJump && data.grounded)
        {
            data.readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), data.jumpCooldown);
        }
    }

    private void MovePlayer()
    {

        // calculate movement direction
        data.moveDirection = orientation.forward * data.verticalInput + orientation.right * data.horizontalInput;

        // on ground
        if (data.grounded)
        {
            rb.AddForce(data.moveDirection.normalized * data.moveSpeed * 10f, ForceMode.Force);
        }


        // in air
        else if (!data.grounded)
            rb.AddForce(data.moveDirection.normalized * data.moveSpeed * 10f * data.airMultiplier, ForceMode.Force);
    }
    private void Run()
    {
        if (Input.GetKey(data.sprintKey))
        {
            data.running = true;
        }
        else
            data.running = false;

        if (data.running)
        {
            data.walking = false;
        }
        else
        {
            data.walking = true;

        }
    }


    private void MouseMovement()
    {

        //rotate camera
        data.turn.x += Input.GetAxis("Mouse X");
        data.turn.y += Input.GetAxis("Mouse Y") * 2;

        transform.localRotation = Quaternion.Euler(0, data.turn.x * data.mouseSpeed, 0);

        //CameraPivot.transform.localRotation = Quaternion.Euler(-turn.y * mouseSpeed, 0, 0);

        data.turn.y = Mathf.Clamp(data.turn.y, data.maxcamDown, data.maxcamUp);
        CameraPivot.transform.eulerAngles = new Vector3(-data.turn.y, data.turn.x * data.mouseSpeed, 0);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > data.moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * data.moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * data.jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        data.readyToJump = true;
    }

    private void CheckForWall()
    {
        wall.wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wall.wallCheckDistance, wall.WhatIsWall);
        wall.wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wall.wallCheckDistance, wall.WhatIsWall);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, wall.minJumpHeight, data.WhatIsGroundL);
    }
    private void StateMachine()
    {
        if ((wall.wallLeft || wall.wallRight) && data.verticalInput > 0 && AboveGround() && data.running && !wall.exitingWall)
        {
            if (!wall.wallrunning)
            {
                StartWallRun();
            }

            if (wall.WallRunTimer > 0)
            {
                wall.WallRunTimer -= Time.deltaTime;
            }

            if(wall.WallRunTimer <= 0 && wall.wallrunning)
            {
                wall.exitingWall = true;
                wall.exitWallTimer = wall.exitWallTime;
            }

            if (Input.GetKeyDown(data.jumpKey))
            {
                WallJump();               
            }
        }
        else if(wall.exitingWall)
        {
            if (wall.wallrunning)
            {
                StopWallRun();
            }

            if (wall.exitWallTimer > 0)
                wall.exitWallTimer -= Time.deltaTime;

            if (wall.exitWallTimer <= 0)
                wall.exitingWall = false;
        }

        else
        {
            if (wall.wallrunning)
            {
                StopWallRun();             
            }
        }
    }

    private void StartWallRun()
    {
        wall.wallrunning = true;

        wall.WallRunTimer = wall.maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        WallCam.DoFov(90f);
        if(wall.wallLeft)
        {
            WallCam.DoTilt(-5f);
        }
        if (wall.wallRight)
        {
            WallCam.DoTilt(5f);
        }
    }
    private void WallRunMovement()
    {
        rb.useGravity = data.useGravity;

        Vector3 wallNormal = wall.wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if ((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        rb.AddForce(wallForward * wall.wallRunForce, ForceMode.Force);

        if (!(wall.wallLeft && data.horizontalInput > 0) && !(wall.wallRight && data.horizontalInput < 0))
            rb.AddForce(-wallNormal * 500, ForceMode.Force);

        if(data.useGravity)
        {
            rb.AddForce(transform.up * data.gravityCounterForce, ForceMode.Force);
        }
    }
    private void StopWallRun()
    {
        float timer = Time.time;
        wall.wallrunning = false;
        WallCam.DoFov(80f);
        WallCam.DoTilt(0f);
    }

    private void WallJump()
    {
        wall.exitingWall = true;
        wall.exitWallTimer = wall.exitWallTime;

        Vector3 wallNormal = wall.wallRight ? rightWallHit.normal : leftWallHit.normal;
        Vector3 forceToAplly = transform.up * wall.wallJumpForce + wallNormal * wall.wallJumpSideForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToAplly, ForceMode.Impulse);
        float timer = Time.time;
    }

}