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

    bool jap;
    bool yank;
    bool ind;
    bool greek;
    bool reg;
    bool blue;
    bool green;
    bool black;

    bool jap2;
    bool yank2;
    bool ind2;
    bool greek2;
    bool reg2;
    bool blue2;
    bool green2;
    bool black2;

    bool jap3;
    bool yank3;
    bool ind3;
    bool greek3;
    bool reg3;
    bool blue3;
    bool green3;
    bool black3;

    bool jap4;
    bool yank4;
    bool ind4;
    bool greek4;
    bool reg4;
    bool blue4;
    bool green4;
    bool black4;

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

        jap = false;
        yank = false;
        ind = false;
        greek = false;
        reg = false;
        blue = false;
        green = false;
        black = false;

        jap2 = false;
        yank2 = false;
        ind2 = false;
        greek2 = false;
        reg2 = false;
        blue2 = false;
        green2 = false;
        black2 = false;

        jap3 = false;
        yank3 = false;
        ind3 = false;
        greek3 = false;
        reg3 = false;
        blue3 = false;
        green3 = false;
        black3 = false;

        jap4 = false;
        yank4 = false;
        ind4 = false;
        greek4 = false;
        reg4 = false;
        blue4 = false;
        green4 = false;
        black4 = false;
    }

    public void Update()
    {
        if(confirms >= players && confirms != 0)
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


    public int getP1Char()
    {
        if (jap == true)
        {
            return 0;
        }
        if (ind == true)
        {
            return 1;
        }
        if (yank == true)
        {
            return 2;
        }
        if (greek == true)
        {
            return 3;
        }
        else {
            return -1;
        }
    }
    public int getP1Col()
    {
        if (reg == true)
        {
            return 0;
        }
        if (green == true)
        {
            return 1;
        }
        if (blue == true)
        {
            return 2;
        }
        if (black == true)
        {
            return 3;
        }
        else
        {
            return -1;
        }
    }
    public int getP2Char()
    {
        if (jap2 == true)
        {
            return 0;
        }
        if (ind2 == true)
        {
            return 1;
        }
        if (yank2 == true)
        {
            return 2;
        }
        if (greek2 == true)
        {
            return 3;
        }
        else
        {
            return -1;
        }
    }
    public int getP3Char()
    {
        if (jap3 == true)
        {
            return 0;
        }
        if (ind3 == true)
        {
            return 1;
        }
        if (yank3 == true)
        {
            return 2;
        }
        if (greek3 == true)
        {
            return 3;
        }
        else
        {
            return -1;
        }
    }
    public int getP4Char()
    {
        if (jap4 == true)
        {
            return 0;
        }
        if (ind4 == true)
        {
            return 1;
        }
        if (yank4 == true)
        {
            return 2;
        }
        if (greek4 == true)
        {
            return 3;
        }
        else
        {
            return -1;
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






    /// <summary>
    /// ////////////
    /// </summary>

    public void Jap()
    {
        jap = true;
        yank = false;
        ind = false;
        greek = false;
    }
    public void Yank()
    {
        jap = false;
        yank = true;
        ind = false;
        greek = false;
    }
    public void Ind()
    {
        jap = false;
        yank = false;
        ind = true;
        greek = false;
    }
    public void Greek()
    {
        jap = false;
        yank = false;
        ind = false;
        greek = true;
    }
    public void Reg()
    {
        reg = true;
        green = false;
        blue = false;
        black = false;
    }
    public void Blue()
    {
        reg = false;
        green = false;
        blue = true;
        black = false;
    }
    public void Green()
    {
        reg = false;
        green = true;
        blue = false;
        black = false;
    }
    public void Black()
    {
        reg = false;
        green = false;
        blue = false;
        black = true;
    }

    /// <summary>
    /// ///////////////////
    /// </summary>


    public void Jap2()
    {
        jap2 = true;
        yank2 = false;
        ind2 = false;
        greek2 = false;
    }
    public void Yank2()
    {
        jap2 = false;
        yank2 = true;
        ind2 = false;
        greek2 = false;
    }
    public void Ind2()
    {
        jap2 = false;
        yank2 = false;
        ind2 = true;
        greek2 = false;
    }
    public void Greek2()
    {
        jap2 = false;
        yank2 = false;
        ind2 = false;
        greek2 = true;
    }
    public void Reg2()
    {
        reg2 = true;
        green2 = false;
        blue2 = false;
        black2 = false;
    }
    public void Blue2()
    {
        reg2 = false;
        green2 = false;
        blue2 = true;
        black2 = false;
    }
    public void Green2()
    {
        reg2 = false;
        green2 = true;
        blue2 = false;
        black2 = false;
    }
    public void Black2()
    {
        reg2 = false;
        green2 = false;
        blue2 = false;
        black2 = true;
    }
    /// <summary>
    /// //////
    /// </summary>

    public void Jap3()
    {
        jap3 = true;
        yank3 = false;
        ind3 = false;
        greek3 = false;
    }
    public void Yank3()
    {
        jap3 = false;
        yank3 = true;
        ind3 = false;
        greek3 = false;
    }
    public void Ind3()
    {
        jap3 = false;
        yank3 = false;
        ind3 = true;
        greek3 = false;
    }
    public void Greek3()
    {
        jap3 = false;
        yank3 = false;
        ind3 = false;
        greek3 = true;
    }
    public void Reg3()
    {
        reg3 = true;
        green3 = false;
        blue3 = false;
        black3 = false;
    }
    public void Blue3()
    {
        reg3 = false;
        green3 = false;
        blue3 = true;
        black3 = false;
    }
    public void Green3()
    {
        reg3 = false;
        green3 = true;
        blue3 = false;
        black3 = false;
    }
    public void Black3()
    {
        reg3 = false;
        green3 = false;
        blue3 = false;
        black3 = true;
    }

    /// <summary>
    /// //////
    /// </summary>
    public void Jap4()
    {
        jap4 = true;
        yank4 = false;
        ind4 = false;
        greek4 = false;
    }
    public void Yank4()
    {
        jap4 = false;
        yank4 = true;
        ind4 = false;
        greek4 = false;
    }
    public void Ind4()
    {
        jap4 = false;
        yank4 = false;
        ind4 = true;
        greek4 = false;
    }
    public void Greek4()
    {
        jap4 = false;
        yank4 = false;
        ind4 = false;
        greek4 = true;
    }
    public void Reg4()
    {
        reg4 = true;
        green4 = false;
        blue4 = false;
        black4 = false;
    }
    public void Blue4()
    {
        reg4 = false;
        green4 = false;
        blue4 = true;
        black4 = false;
    }
    public void Green4()
    {
        reg4 = false;
        green4 = true;
        blue4 = false;
        black4 = false;
    }
    public void Black4()
    {
        reg4 = false;
        green4 = false;
        blue4 = false;
        black4 = true;
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

    public void Unconfirm()
    {
        confirms--;
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
