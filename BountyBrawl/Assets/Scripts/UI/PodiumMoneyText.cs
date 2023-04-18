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
    [SerializeField] TMP_Text spacer;
    [SerializeField] Color[] colors;
    [HideInInspector] public int medalPlacement;
    [SerializeField] private CameraShake camera;
    [SerializeField] private float cameraShakeStrength = 0.5f;
    [SerializeField] private GameObject medal;

    private StatTracker[] stats;
    private int playerIndex;
    private bool statTurn; //If the poster is supposed to show stats
    private bool showingStats; //If the poster is currently showing stats

    private AudioSource shot;

    void Start()
    {
        if (medal != null)
        {
            medal.SetActive(false);
        }
        stats = FindObjectsOfType<StatTracker>();
        shot = GetComponent<AudioSource>();
        showingStats = false;
        playerIndex = 5;
        medalPlacement = 1;

        if(medalPlacement == place)
        {
            statTurn = true;
        }

        WinnerMoney();
    }

    private void Update()
    {
        if(statTurn && !showingStats)
        {
            showingStats = true;

            StartCoroutine(ShowingStats());
        }   
    }

    public void WinnerMoney()
    {
        //Debug.Log(place);

        if (place == 4)
        {
            BountyText.text = "$" + ((int)Random.Range(2, 10)).ToString("n0");
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
            BountyText.text = "$" + ((int)Random.Range(20000, 50000)).ToString("n0");
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
            BountyText.text = "$" + ((int)Random.Range(200000, 600000)).ToString("n0");
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
            BountyText.text = "$" + ((int)Random.Range(1200000, 2000000)).ToString("n0");
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

        /*
        Debug.Log("Place: " + place);

        for (int i = 0; i < stats.Length; i++)
        {
            Debug.Log("Placements " + i + ": "+ stats[i].placement);
        }

        Debug.Log("Playerindex " + playerIndex);
        */

        Stats();
    }

    public void Stats()
    {
        Debug.Log(playerIndex);

        if (playerIndex == 0)
        {
            spacer.color = colors[0];
            KillText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetKills().ToString() + " Confirmed Kills";
            LostText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetLost().ToString() + " Lost Cleansed";
            PlayerDamageText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetDamage().ToString() + " Player Damage";
            LostDamageText.text = GameObject.FindGameObjectWithTag("P1").GetComponent<StatTracker>().GetLostDamage().ToString() + " Lost Damage";
            Dissapear();
        }
        if (playerIndex == 1)
        {
            spacer.color = colors[1];
            KillText.text = GameObject.FindGameObjectWithTag("P2").GetComponent<StatTracker>().GetKills().ToString() + " Confirmed Kills";
            LostText.text = GameObject.FindGameObjectWithTag("P2").GetComponent<StatTracker>().GetLost().ToString() + " Lost Cleansed";
            PlayerDamageText.text = GameObject.FindGameObjectWithTag("P2").GetComponent<StatTracker>().GetDamage().ToString() + " Player Damage";
            LostDamageText.text = GameObject.FindGameObjectWithTag("P2").GetComponent<StatTracker>().GetLostDamage().ToString() + " Lost Damage";
            Dissapear();
        }
        if (playerIndex == 2)
        {
            spacer.color = colors[2];
            KillText.text = GameObject.FindGameObjectWithTag("P3").GetComponent<StatTracker>().GetKills().ToString() + " Confirmed Kills";
            LostText.text = GameObject.FindGameObjectWithTag("P3").GetComponent<StatTracker>().GetLost().ToString() + " Lost Cleansed";
            PlayerDamageText.text = GameObject.FindGameObjectWithTag("P3").GetComponent<StatTracker>().GetDamage().ToString() + " Player Damage";
            LostDamageText.text = GameObject.FindGameObjectWithTag("P3").GetComponent<StatTracker>().GetLostDamage().ToString() + " Lost Damage";
            Dissapear();
        }
        if (playerIndex == 3)
        {
            spacer.color = colors[3];
            KillText.text = GameObject.FindGameObjectWithTag("P4").GetComponent<StatTracker>().GetKills().ToString() + " Confirmed Kills";
            LostText.text = GameObject.FindGameObjectWithTag("P4").GetComponent<StatTracker>().GetLost().ToString() + " Lost Cleansed";
            PlayerDamageText.text = GameObject.FindGameObjectWithTag("P4").GetComponent<StatTracker>().GetDamage().ToString() + " Player Damage";
            LostDamageText.text = GameObject.FindGameObjectWithTag("P4").GetComponent<StatTracker>().GetLostDamage().ToString() + " Lost Damage";
            Dissapear();
        }
    }

    private void Dissapear()
    {
        KillText.gameObject.SetActive(false);
        LostText.gameObject.SetActive(false);
        PlayerDamageText.gameObject.SetActive(false);
        LostDamageText.gameObject.SetActive(false);
    }

    private IEnumerator ShowingStats()
    {
        yield return new WaitForSeconds(0.3f);
        KillText.gameObject.SetActive(true);
        shot.Play();
        camera.setShake(true);
        camera.StartCoroutine(camera.Shake(0.1f, 0.5f));

        yield return new WaitForSeconds(0.3f);
        LostText.gameObject.SetActive(true);
        shot.Play();
        camera.setShake(true);
        camera.StartCoroutine(camera.Shake(0.1f, 0.5f));

        yield return new WaitForSeconds(0.3f);
        PlayerDamageText.gameObject.SetActive(true);
        shot.Play();
        camera.setShake(true);
        camera.StartCoroutine(camera.Shake(0.1f, 0.5f));

        yield return new WaitForSeconds(0.3f);
        LostDamageText.gameObject.SetActive(true);
        shot.Play();
        camera.setShake(true);
        camera.StartCoroutine(camera.Shake(0.1f, 0.5f));

        yield return new WaitForSeconds(0.3f);
        if (medal != null)
        {
            medal.gameObject.SetActive(true);
        }
    }

    public void PlayMedalMove()
    {
        medalPlacement++;

        if (medalPlacement == place)
        {
            statTurn = true;
        }
    }
}
