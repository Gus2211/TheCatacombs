using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer General;

    public MoveData data;
    public AudioSource Music;
    public GameObject WinCanvas;
    public GameObject TutorialCanvas;
    public GameObject QualityCanvas;
    public GameObject AudioSettingsCanvas;
    public GameObject RespawnLoc;
    public GameObject HudCanvas;
    public GameObject GameOverCanvas;
    public GameObject MainMenuCanvas;
    public GameObject OptionsCanvas;
    public PlayerMovement playerS;
    public void Restart()
    {
        GameOverCanvas.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerS.MouseMove = true;
        Time.timeScale = 1f;
        TutorialCanvas.SetActive(true);
        WinCanvas.SetActive(false);
        Music.Play();
    }
    public void MainMenu()
    {
        GameOverCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        playerS.MouseMove = false;
        WinCanvas.SetActive(false);
    }

    public void StartGame()
    {
        MainMenuCanvas.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerS.MouseMove = true;
        Time.timeScale = 1f;
        HudCanvas.SetActive(true);
        Music.Play();

        this.transform.localRotation = Quaternion.Euler(0, 0, 0);
        playerS.CameraPivot.transform.localRotation = Quaternion.Euler(0, 0, 0);
        TutorialCanvas.SetActive(true);
    }
    
    public void Options()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        OptionsCanvas.SetActive(true);
        MainMenuCanvas.SetActive(false);
    }
    public void Audio()
    {
        OptionsCanvas.SetActive(false);
        AudioSettingsCanvas.SetActive(true);
    }
    public void MenuBack()
    {
        OptionsCanvas.SetActive(false);
        MainMenuCanvas.SetActive(true);
        WinCanvas.SetActive(false);
    }
    public void OptionsBack()
    {
        AudioSettingsCanvas.SetActive(false);
        QualityCanvas.SetActive(false);
        OptionsCanvas.SetActive(true);
    }
    public void Quality()
    {
        QualityCanvas.SetActive(true);
        OptionsCanvas.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void OnChangeSliderMouse(float value)
    {
        data.mouseSpeed = value;
    }

    public void OnChangeSliderGeneral(float value)
    {
        General.SetFloat("General", Mathf.Log10(value) * 20);
    }
    public void OnChangeSliderMusic(float value)
    {
        General.SetFloat("Music", Mathf.Log10(value) * 20);
    }
    public void OnChangeSliderSFX(float value)
    {
        General.SetFloat("SFX", Mathf.Log10(value) * 20);
    }
}
