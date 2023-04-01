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

    int winner;
    [SerializeField]
    GameObject podSand;

    [SerializeField]
    GameObject podJap;

    [SerializeField]
    GameObject podEmr;

    [SerializeField]
    GameObject podGreek;

    bool map1;
    bool map2;
    int players;
    int confirms;



    public void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void Start()
    {
        map1 = false;
        map2 = false;
        players = 0;
        confirms = 0;

        
    }

    public void Update()
    {
        
        if(confirms == players && confirms != 0)
        {
            if (map1 == true)
            {
                
                confirms = 0;
                map1 = false;
                map2 = false;

                StartCoroutine(TheAssEater5000(1));
            }
            if (map2 == true)
            {
                
                confirms = 0;
                map1 = false;
                map2 = false;

                StartCoroutine(TheAssEater5000(2));
            }
        }

        
    }

   

    public void SetWinner(int number)
    {
        if (winner == 0)
        {
            
        }

        if (winner == 1)
        {
            podJap.SetActive(true);
        }

        if (winner == 2)
        {
            podEmr.SetActive(true);
        }

        if (winner == 3)
        {
            podGreek.SetActive(true);
        }

        if (winner == 4)
        {
            podSand.SetActive(true);
        }
    }


   

    public void Players1()
    {
        players = 1;
    }
    public void Players2()
    {
        players = 2;
    }
    public void Players3()
    {
        players = 3;
    }
    public void Players4()
    {
        players = 4;
    }

    public int GetPlayers()
    {
        return players;
    }


    ////////////////////////////
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
        map1 = true;
        map2 = false;
    }

    public void Map2()
    {
        map2 = true;
        map1 = false;
    }

    public void Confirm()
    {
        confirms++;
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

    public void Podium()
    {
        SceneManager.LoadScene(3);
    }
}
