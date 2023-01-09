using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WallRunCam : MonoBehaviour
{
    public GameObject leverO;
    public Lever lever;
    public float interactRange = 500f;
    public AudioSource LeverP;
    public KeyCode Interact = KeyCode.E;
    public float doordelay = 1.5f;
    private float doortime;
    private bool COpen;
    private RaycastHit hit;
    public Animation LPull;



    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }




    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.E))
        {   
            
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRange) && hit.transform.tag == "Lever")
            {
                Debug.DrawLine(transform.position, hit.transform.position, Color.green, interactRange);
                LeverP.Play();
                leverO = hit.transform.gameObject;
                LPull = leverO.transform.GetComponent<Animation>();
                LeverP = leverO.transform.GetComponent<AudioSource>();
                LPull.Play();  
                doortime = Time.time + doordelay;
                COpen = true;
                leverO.transform.tag = "Used";
            }
        }
        if (Time.time > doortime && COpen)
        {
            OpenDoor();
        }
    }
    private void OpenDoor()
    {
        COpen = false;  
        lever = hit.transform.GetComponent<Lever>();
        lever.OpenDoor();
    }
}
