using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelected : MonoBehaviour
{
    [SerializeField]
    public GameObject sand;
    [SerializeField]
    public GameObject nag;
    [SerializeField]
    public GameObject emr;
    [SerializeField]
    public GameObject greed;

    public int playerColour;
    public int playerChar;

    public void SandGrabColour()
    {
        playerColour = sand.GetComponent<ColourSelect>().GetColour();
        playerChar = 0;
    }
    public void NagGrabColour()
    {
        playerColour = nag.GetComponent<ColourSelect>().GetColour();
        playerChar = 1;
    }
    public void EmrGrabColour()
    {
        playerColour = emr.GetComponent<ColourSelect>().GetColour();
        playerChar = 2;
    }
    public void GreedGrabColour()
    {
        playerColour = greed.GetComponent<ColourSelect>().GetColour();
        playerChar = 3;
    }

    public int GetPlayerColour()
    {
        return playerColour;
    }
    public int GetPlayerChar()
    {
        return playerChar;
    }
}
