using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool IsGamePaused = false;
    public GameObject pauseMenuUI;
    public GameObject timer_text;
    public GameObject LeftFreeze;
    public GameObject RightFreeze;
    public GameObject x2Freeze;
    

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        foreach (PaddleController paddle in FindObjectsOfType<PaddleController>()){
            paddle.EnableControls(0);
        }
        IsGamePaused = false;
        timer_text.SetActive(true);
        LeftFreeze.SetActive(true);
        RightFreeze.SetActive(true);
        x2Freeze.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        foreach (PaddleController paddle in FindObjectsOfType<PaddleController>()){
            paddle.DisableControls();
        }
        IsGamePaused = true;
        timer_text.SetActive(false);
        LeftFreeze.SetActive(false);
        RightFreeze.SetActive(false);
        x2Freeze.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Scenes/Menu");
        Time.timeScale = 1f;
        foreach (PaddleController paddle in FindObjectsOfType<PaddleController>()){
            paddle.EnableControls(0);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
