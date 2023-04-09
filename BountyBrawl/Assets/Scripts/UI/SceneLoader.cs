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

    int deadPlayers;

    int Index4th;
    int Char4th;
    int colour4th;

    int Index3rd;
    int Char3rd;
    int colour3rd;

    int Index2nd;
    int Char2nd;
    int colour2nd;

    int Index1st;
    int Char1st;
    int colour1st;

    private bool AllDead;

    public void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        
    }

    public void Start()
    {
        AllDead = false;
        map1 = false;
        map2 = false;
        players = 0;
        confirms = 0;

        deadPlayers = 0;


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


    public void DeadPlayer(int index)
    {
        if (deadPlayers == 0)
        {
            Index4th = index;
           
            deadPlayers = 1;
        }
        if (deadPlayers == 1)
        {
            Index3rd = index;
          
            deadPlayers = 2;
        }
        if (deadPlayers == 2)
        {
            Index2nd = index;
            
            deadPlayers = 3;
        }
        
    }
    public void CheckAmountRemaining()
    {
        if(deadPlayers >= players - 1 && !AllDead)
        {
            AllDead = true;
            StartCoroutine(WaitPodium());
        }
    }

    private IEnumerator WaitPodium()
    {
        yield return new WaitForSeconds(2f);
        Podium();
    }

    public int Get4th()
    {
        return Index4th;
    }
    public int Get3rd()
    {
        return Index3rd;
    }
    public int Get2nd() 
    { 
        return Index2nd;
    }


    public void PodiumAmount()
    {
        if(players < 4)
        {
            GameObject.Find("Poster 4").SetActive(false);
        }
        if (players < 3)
        {
            GameObject.Find("Poster 3").SetActive(false);
        }
        if (players < 2)
        {
            GameObject.Find("Poster 2").SetActive(false);
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
        int current = SceneManager.GetActiveScene().buildIndex;
        SceneManager.UnloadSceneAsync(current);
        SceneManager.LoadScene(current);
        pauseMenu.GetComponent<PauseScript>().ResumeGame();
        Debug.Log("Restart");

        AllDead = false;
        map1 = false;
        map2 = false;
        players = 0;
        confirms = 0;

        deadPlayers = 0;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        PauseScript.isPaused = false;

        AllDead = false;
        map1 = false;
        map2 = false;
        players = 0;
        confirms = 0;

        deadPlayers = 0;
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
        SceneManager.LoadScene(4);
        PodiumAmount();

    }
}
