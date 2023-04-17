using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PodiumMoneyText : MonoBehaviour
{
    [SerializeField]
    private int place;


    public TMP_Text BountyText, KillText, LostText, PlayerDamageText, LostDamageText;

    [SerializeField] private Transform[] posterPositions;

    private StatTracker[] stats;
    private int playerIndex;

    void Start()
    {
        stats = FindObjectsOfType<StatTracker>();
        playerIndex = 5;

        WinnerMoney();
    }

    public void WinnerMoney()
    {
        //Debug.Log(place);

        if (place == 4)
        {
            BountyText.text = "$6";
            transform.position = posterPositions[3].position;

            for(int i = 0; i < stats.Length; i++)
            {
                if(stats[i].placement == place)
                {
                    playerIndex = stats[i].playerIndex;
                    break;
                }
            }
        }
        if (place == 3)
        {
            BountyText.text = "$46,000";
            transform.position = posterPositions[2].position;

            for (int i = 0; i < stats.Length; i++)
            {
                if (stats[i].placement == place)
                {
                    playerIndex = stats[i].playerIndex;
                    break;
                }
            }
        }
        if (place == 2)
        {
            BountyText.text = "$275,000";
            transform.position = posterPositions[1].position;

            for (int i = 0; i < stats.Length; i++)
            {
                if (stats[i].placement == place)
                {
                    playerIndex = stats[i].playerIndex;
                    break;
                }
            }
        }
        if (place == 1)
        {
            BountyText.text = "$1,849,000";
            transform.position = posterPositions[0].position;

            for (int i = 0; i < stats.Length; i++)
            {
                if (stats[i].placement == place)
                {
                    playerIndex = stats[i].playerIndex;
                    break;
                }
            }
        }

        Debug.Log("Place: " + place);

        for (int i = 0; i < stats.Length; i++)
        {
            Debug.Log("Placements " + i + ": "+ stats[i].placement);
        }

        Debug.Log("Playerindex " + playerIndex);

        Stats();
    }

    public void Stats()
    {
        Debug.Log(playerIndex);

        if (playerIndex == 0)
        {
            KillText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetKills().ToString() + " Confirmed Kills";
            LostText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetLost().ToString() + " Lost Cleansed";
            PlayerDamageText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetDamage().ToString() + " Player Damage";
            LostDamageText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetLostDamage().ToString() + " Lost Damage";
        }
        if (playerIndex == 1)
        {
            KillText.text = GameObject.FindGameObjectWithTag("P2").GetComponent<StatTracker>().GetKills().ToString() + " Confirmed Kills";
            LostText.text = GameObject.FindGameObjectWithTag("P2").GetComponent<StatTracker>().GetLost().ToString() + " Lost Cleansed";
            PlayerDamageText.text = GameObject.FindGameObjectWithTag("P2").GetComponent<StatTracker>().GetDamage().ToString() + " Player Damage";
            LostDamageText.text = GameObject.FindGameObjectWithTag("P2").GetComponent<StatTracker>().GetLostDamage().ToString() + " Lost Damage";
        }
        if (playerIndex == 2)
        {
            KillText.text = GameObject.FindGameObjectWithTag("P3").GetComponent<StatTracker>().GetKills().ToString() + " Confirmed Kills";
            LostText.text = GameObject.FindGameObjectWithTag("P3").GetComponent<StatTracker>().GetLost().ToString() + " Lost Cleansed";
            PlayerDamageText.text = GameObject.FindGameObjectWithTag("P3").GetComponent<StatTracker>().GetDamage().ToString() + " Player Damage";
            LostDamageText.text = GameObject.FindGameObjectWithTag("P3").GetComponent<StatTracker>().GetLostDamage().ToString() + " Lost Damage";
        }
        if (playerIndex == 3)
        {
            KillText.text = GameObject.FindGameObjectWithTag("P4").GetComponent<StatTracker>().GetKills().ToString() + " Confirmed Kills";
            LostText.text = GameObject.FindGameObjectWithTag("P4").GetComponent<StatTracker>().GetLost().ToString() + " Lost Cleansed";
            PlayerDamageText.text = GameObject.FindGameObjectWithTag("P4").GetComponent<StatTracker>().GetDamage().ToString() + " Player Damage";
            LostDamageText.text = GameObject.FindGameObjectWithTag("P4").GetComponent<StatTracker>().GetLostDamage().ToString() + " Lost Damage";
        }
    }
}
