using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1setter : MonoBehaviour
{
    GameObject loader;

    int p1char;
    int p1colour;

    [SerializeField]
    GameObject sandreg;
    [SerializeField]
    GameObject sandgreen;
    [SerializeField]
    GameObject sandblue;
    [SerializeField]
    GameObject sandred;
    [SerializeField]
    GameObject emrreg;
    [SerializeField]
    GameObject emrgreen;
    [SerializeField]
    GameObject emrblue;
    [SerializeField]
    GameObject emrred;
    [SerializeField]
    GameObject nagreg;
    [SerializeField]
    GameObject naggreen;
    [SerializeField]
    GameObject nagblue;
    [SerializeField]
    GameObject nagred;
    [SerializeField]
    GameObject greedreg;
    [SerializeField]
    GameObject greedgreen;
    [SerializeField]
    GameObject greedblue;
    [SerializeField]
    GameObject greedred;

    // Start is called before the first frame update
    void Start()
    {
        loader = GameObject.Find("MainSceneLoader");
        p1colour = loader.GetComponent<SceneLoader>().GetP1Colour();
        p1char = loader.GetComponent<SceneLoader>().GetP1Char();

        if(p1char == 0)
        {
            if(p1colour == 0)
            {
                sandreg.SetActive(true);
            }
            if (p1colour == 1)
            {
                sandgreen.SetActive(true);
            }
            if (p1colour == 2)
            {
                sandblue.SetActive(true);
            }
            if (p1colour == 3)
            {
                sandred.SetActive(true);
            }
        }
        if (p1char == 1)
        {
            if (p1colour == 0)
            {
                nagreg.SetActive(true);
            }
            if (p1colour == 1)
            {
                naggreen.SetActive(true);
            }
            if (p1colour == 2)
            {
                nagblue.SetActive(true);
            }
            if (p1colour == 3)
            {
                nagred.SetActive(true);
            }
        }
        if (p1char == 2)
        {
            if (p1colour == 0)
            {
                emrreg.SetActive(true);
            }
            if (p1colour == 1)
            {
                emrgreen.SetActive(true);
            }
            if (p1colour == 2)
            {
                emrblue.SetActive(true);
            }
            if (p1colour == 3)
            {
                emrred.SetActive(true);
            }
        }
        if (p1char == 3)
        {
            if (p1colour == 0)
            {
                greedreg.SetActive(true);
            }
            if (p1colour == 1)
            {
                greedgreen.SetActive(true);
            }
            if (p1colour == 2)
            {
                greedblue.SetActive(true);
            }
            if (p1colour == 3)
            {
                greedred.SetActive(true);
            }
        }
    }

    
}
