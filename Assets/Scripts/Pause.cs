using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseText;
    public GameObject mainMenuButton;
    public GameObject exitButton;
    public GameObject resetButton;
    private void OnPause()
    {
       if(!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0.0f;
            pauseText.SetActive(true); mainMenuButton.SetActive(true); resetButton.SetActive(true); exitButton.SetActive(true);
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1.0f;
            pauseText.SetActive(false); mainMenuButton.SetActive(false); exitButton.SetActive(false); resetButton.SetActive(false);
        }
    }
}
