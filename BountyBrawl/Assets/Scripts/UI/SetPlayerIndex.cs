using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetPlayerIndex : MonoBehaviour
{
    GameObject info;
    int p1char;
    int p2char;
    int p3char;
    int p4char;

    bool nagDead;
    bool sandDead;
    bool emrDead;
    bool greedDead;

    [SerializeField]
    public GameObject sand;
    [SerializeField]
    public GameObject nag;
    [SerializeField]
    public GameObject greed;
    [SerializeField]
    public GameObject emr;

    //0 nag, 1 emr, 2 sand, 3 greed

    // Start is called before the first frame update
    void Start()
    {
        info = GameObject.Find("MainSceneLoader");
        p1char = info.GetComponent<SceneLoader>().getP1Char();
        p2char = info.GetComponent<SceneLoader>().getP2Char();
        p3char = info.GetComponent<SceneLoader>().getP3Char();
        p4char = info.GetComponent<SceneLoader>().getP4Char();

        nag.GetComponent<PlayerBody>().SetPlayerIndex(-1);
        emr.GetComponent<PlayerBody>().SetPlayerIndex(-1);
        sand.GetComponent<PlayerBody>().SetPlayerIndex(-1);
        greed.GetComponent<PlayerBody>().SetPlayerIndex(-1);

        if (p1char == 0)
        {
            nag.GetComponent<PlayerBody>().SetPlayerIndex(0);
        }
        if (p1char == 1)
        {
            emr.GetComponent<PlayerBody>().SetPlayerIndex(0);
        }
        if (p1char == 2)
        {
            sand.GetComponent<PlayerBody>().SetPlayerIndex(0);
        }
        if (p1char == 3)
        {
            greed.GetComponent<PlayerBody>().SetPlayerIndex(0);
        }
        Debug.Log(p1char);
        Debug.Log(p2char);
        ///

        if (p2char == 0)
        {
            nag.GetComponent<PlayerBody>().SetPlayerIndex(1);
        }
        if (p2char == 1)
        {
            emr.GetComponent<PlayerBody>().SetPlayerIndex(1);
        }
        if (p2char == 2)
        {
            sand.GetComponent<PlayerBody>().SetPlayerIndex(1);
        }
        if (p2char == 3)
        {
            greed.GetComponent<PlayerBody>().SetPlayerIndex(1);
        }

        ////

        if (p3char == 0)
        {
            nag.GetComponent<PlayerBody>().SetPlayerIndex(2);
        }
        if (p3char == 1)
        {
            emr.GetComponent<PlayerBody>().SetPlayerIndex(2);
        }
        if (p3char == 2)
        {
            sand.GetComponent<PlayerBody>().SetPlayerIndex(2);
        }
        if (p3char == 3)
        {
            greed.GetComponent<PlayerBody>().SetPlayerIndex(2);
        }

        ///

        if (p4char == 0)
        {
            nag.GetComponent<PlayerBody>().SetPlayerIndex(3);
        }
        if (p4char == 1)
        {
            emr.GetComponent<PlayerBody>().SetPlayerIndex(3);
        }
        if (p4char == 2)
        {
            sand.GetComponent<PlayerBody>().SetPlayerIndex(3);
        }
        if (p4char == 3)
        {
            greed.GetComponent<PlayerBody>().SetPlayerIndex(3);
        }
    }

    // Update is called once per frame
    void Update()
    {
       


        if (nag.GetComponent<PlayerBody>().getDead() == true)
        {
            nagDead = true;
        }
        if (sand.GetComponent<PlayerBody>().getDead() == true)
        {
            sandDead = true;
        }
        if (emr.GetComponent<PlayerBody>().getDead() == true)
        {
            emrDead = true;
        }
        if (greed.GetComponent<PlayerBody>().getDead() == true)
        {
            greedDead = true;
        }

        ///

        if (nagDead == true && sandDead == true && emrDead == true && greedDead == true)
        {
            info.GetComponent<SceneLoader>().SetWinner(0);
        }
        ///

        if (nagDead == true && sandDead == true && emrDead == true) //greed
        {
            float JapMoney = nag.GetComponent<StatTracker>().GetCurrMoney();
            float EmrMoney = emr.GetComponent<StatTracker>().GetCurrMoney();
            float SandMoney = sand.GetComponent<StatTracker>().GetCurrMoney();
            float GreekMoney = greed.GetComponent<StatTracker>().GetCurrMoney();

            ReturnMoney(JapMoney, EmrMoney, SandMoney, GreekMoney);
            
        }
        if (nagDead == true && sandDead == true && greedDead == true) //emr
        {
            float JapMoney = nag.GetComponent<StatTracker>().GetCurrMoney();
            float EmrMoney = emr.GetComponent<StatTracker>().GetCurrMoney();
            float SandMoney = sand.GetComponent<StatTracker>().GetCurrMoney();
            float GreekMoney = greed.GetComponent<StatTracker>().GetCurrMoney();

            ReturnMoney(JapMoney, EmrMoney, SandMoney, GreekMoney);


        }
        if (nagDead == true  && emrDead == true && greedDead == true) ///sand
        {
            float JapMoney = nag.GetComponent<StatTracker>().GetCurrMoney();
            float EmrMoney = emr.GetComponent<StatTracker>().GetCurrMoney();
            float SandMoney = sand.GetComponent<StatTracker>().GetCurrMoney();
            float GreekMoney = greed.GetComponent<StatTracker>().GetCurrMoney();

            ReturnMoney(JapMoney, EmrMoney, SandMoney, GreekMoney);

        }
        if (sandDead == true && emrDead == true && greedDead == true) ///nag
        {
            float JapMoney = nag.GetComponent<StatTracker>().GetCurrMoney();
            float EmrMoney = emr.GetComponent<StatTracker>().GetCurrMoney();
            float SandMoney = sand.GetComponent<StatTracker>().GetCurrMoney();
            float GreekMoney = greed.GetComponent<StatTracker>().GetCurrMoney();

            ReturnMoney(JapMoney, EmrMoney, SandMoney, GreekMoney);

        }




    }

    public void ReturnMoney(float one, float two, float three, float four)
    {

        if (one > two && two > three && three > four)
        {
            info.GetComponent<SceneLoader>().SetWinner(1);
        }
        if (two > one && one > three && three > four)
        {
            info.GetComponent<SceneLoader>().SetWinner(2);
        }
        if (three > two && two > one && one > four)
        {
            info.GetComponent<SceneLoader>().SetWinner(3);
        }
        if (four > two && two > three && three > one)
        {
            info.GetComponent<SceneLoader>().SetWinner(4);
        }
    }
}
