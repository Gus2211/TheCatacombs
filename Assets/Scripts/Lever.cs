using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public AudioSource ODoor;
    public Animation Door;

    public void OpenDoor()
    {
        Door = Door.transform.GetComponent<Animation>();
        ODoor = ODoor.transform.GetComponent<AudioSource>();
        Door.Play();
        if (Door.isPlaying)
        {
            ODoor.Play();
        }
    }
}
