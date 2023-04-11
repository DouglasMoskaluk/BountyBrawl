using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PodiumMoneyText : MonoBehaviour
{
    [SerializeField]
    private int index;


    public TMP_Text BountyText, MoneyText, KillText, LostText, EaterText, PlayerDamageText, LostDamageText;

    void Start()
    {
        
        WinnerMoney();
        Stats();
    }

    public void WinnerMoney()
    {
        Debug.Log(index);


        if (index == GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().Get4th())
        {
            BountyText.text = "$6";
        }
        if (index == GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().Get3rd())
        {
            BountyText.text = "$46,000";
        }
        if (index == GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().Get2nd())
        {
            BountyText.text = "$275,000";
        }
        else
        {
            BountyText.text = "$1,849,000";
        }
    }

    public void Stats()
    {
        if (index == 0)
        {
            MoneyText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetMoney().ToString() + " Money Earned";
            KillText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetKills().ToString() + " Confirmed Kills";
            LostText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetLost().ToString() + " Lost Cleansed";
            EaterText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetEater().ToString() + " Eater Kills";
            PlayerDamageText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetDamage().ToString() + " Player Damage";
            LostDamageText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetLostDamage().ToString() + " Lost Damage";
        }
    }
}
