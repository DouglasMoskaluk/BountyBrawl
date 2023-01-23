using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseScript : MonoBehaviour
{
    public GameObject PauseMenu;
    public static bool isPaused;
    

    void Start()
    {
        PauseMenu.SetActive(false);
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PressedPause()
    {
        Debug.Log("Pause SCripts");
        if (isPaused)
        {
            ResumeGame();
            Debug.Log("2");
        }
        else
        {
            PauseGame();
            Debug.Log("1");
        }
    }

}
