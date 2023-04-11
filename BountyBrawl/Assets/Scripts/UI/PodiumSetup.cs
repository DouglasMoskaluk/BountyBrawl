using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PodiumSetup : MonoBehaviour
{
    int players;
    [SerializeField]
    GameObject P2, P3, P4;
    void Start()
    {
        players = GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().GetPlayers();

        if(players <= 3)
        {
            P4.SetActive(false);
        }
        if (players <= 2)
        {
            P3.SetActive(false);
        }
        if (players == 1)
        {
            P2.SetActive(false);
        }
    }

   
}
