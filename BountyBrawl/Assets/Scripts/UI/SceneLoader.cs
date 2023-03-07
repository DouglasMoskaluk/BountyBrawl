using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    GameObject loadingScreenobj;
    int loadTime;

    [SerializeField]
    GameObject pauseMenu;

    public void Start()
    {

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    
    IEnumerator TheAssEater5000(int index) //Loading screen
    {
        loadTime = loadingScreenobj.GetComponent<LoadingScreen>().randTime;
        yield return new WaitForSeconds(loadTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + index);
    }

    public void Map1()
    {
        StartCoroutine(TheAssEater5000(1));
    }

    public void Map2()
    {

        StartCoroutine(TheAssEater5000(2));
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        pauseMenu.GetComponent<PauseScript>().ResumeGame();
        Debug.Log("Restart");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        PauseScript.isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }    

    public void TestScene()
    {
        SceneManager.LoadScene(0);
    }
}
