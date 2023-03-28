using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayers : MonoBehaviour
{
    GameObject loader;

    int p1char;
    int p1colour;
    int p2char;
    int p2colour;
    int p3char;
    int p3colour;
    int p4char;
    int p4colour;

    // Start is called before the first frame update
    void Start()
    {
        loader = GameObject.Find("MainSceneLoader");

        p1char = loader.GetComponent<SceneLoader>().GetP1Char();
        p2char = loader.GetComponent<SceneLoader>().GetP1Char();
        p3char = loader.GetComponent<SceneLoader>().GetP1Char();
        p4char = loader.GetComponent<SceneLoader>().GetP1Char();
        p1colour = loader.GetComponent<SceneLoader>().GetP1Colour();
        p2colour = loader.GetComponent<SceneLoader>().GetP2Colour();
        p3colour = loader.GetComponent<SceneLoader>().GetP3Colour();
        p4colour = loader.GetComponent<SceneLoader>().GetP4Colour();



    }

    
}
