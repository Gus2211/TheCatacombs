using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Death : MonoBehaviour
{
    public string player;
    public GameObject pSpawn;
    public GameObject GameOverCanvas;
    public bool MouseMove;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == player)
        {
            other.transform.position = pSpawn.transform.position;
            GameOverCanvas.SetActive(true);
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            MouseMove = false;
        }
    }
}
