using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrap : MonoBehaviour
{
    public float rspeed = 20.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rspeed * Time.deltaTime);
    }
}
