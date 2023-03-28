using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1setter : MonoBehaviour
{
    GameObject loader;

    int p1char;
    int p1colour;

    [SerializeField]
    GameObject sand;
    [SerializeField]
    GameObject emr;
    [SerializeField]
    GameObject nag;
    [SerializeField]
    GameObject greed;

    // Start is called before the first frame update
    void Start()
    {

        loader = GameObject.Find("MainSceneLoader");
        p1colour = loader.GetComponent<SceneLoader>().GetP1Colour();
        p1char = loader.GetComponent<SceneLoader>().GetP1Char();

        Debug.Log(p1colour);
        Debug.Log(p1char);

        if (p1char == 0)
        {
            sand.SetActive(true);
            if(p1colour == 0)
            {
                sand.GetComponent<PlayerBody>().SetPlayerSkin(0);
            }
            if (p1colour == 1)
            {
                sand.GetComponent<PlayerBody>().SetPlayerSkin(1);
            }
            if (p1colour == 2)
            {
                sand.GetComponent<PlayerBody>().SetPlayerSkin(2);
            }
            if (p1colour == 3)
            {
                sand.GetComponent<PlayerBody>().SetPlayerSkin(3);
            }
        }
        if (p1char == 1)
        {
            nag.SetActive(true);
            if (p1colour == 0)
            {
                nag.GetComponent<PlayerBody>().SetPlayerSkin(0);
            }
            if (p1colour == 1)
            {
                nag.GetComponent<PlayerBody>().SetPlayerSkin(1);
            }
            if (p1colour == 2)
            {
                nag.GetComponent<PlayerBody>().SetPlayerSkin(2);
            }
            if (p1colour == 3)
            {
                nag.GetComponent<PlayerBody>().SetPlayerSkin(3);
            }
        }
        if (p1char == 2)
        {
            emr.SetActive(true);
            if (p1colour == 0)
            {
                emr.GetComponent<PlayerBody>().SetPlayerSkin(0);
            }
            if (p1colour == 1)
            {
                emr.GetComponent<PlayerBody>().SetPlayerSkin(1);
            }
            if (p1colour == 2)
            {
                emr.GetComponent<PlayerBody>().SetPlayerSkin(2);
            }
            if (p1colour == 3)
            {
                emr.GetComponent<PlayerBody>().SetPlayerSkin(3);
            }
        }
        if (p1char == 3)
        {
            greed.SetActive(true);
            if (p1colour == 0)
            {
                greed.GetComponent<PlayerBody>().SetPlayerSkin(0);
            }
            if (p1colour == 1)
            {
                greed.GetComponent<PlayerBody>().SetPlayerSkin(1);
            }
            if (p1colour == 2)
            {
                greed.GetComponent<PlayerBody>().SetPlayerSkin(2);
            }
            if (p1colour == 3)
            {
                greed.GetComponent<PlayerBody>().SetPlayerSkin(3);
            }
        }
    }

    
}
