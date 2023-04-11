using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class P1 : MonoBehaviour
{
    GameObject P1Char;
    bool confirmed;

    [SerializeField]
    GameObject infoSand, infoNag, infoEmr, infoGreed;

    int P1CharSelect;
    int P1SkinSelect;

    public void Awake()
    {
        
        DontDestroyOnLoad(transform.gameObject);
        
    }
    public void Start()
    {
        confirmed = false;
        

    }
    // Update is called once per frame
    void Update()
    {
        P1Char = GameObject.FindGameObjectWithTag("P1 Char");

    }

    public void TriangleTrue()
    {
        infoSand.SetActive(true);
        infoNag.SetActive(true);
        infoEmr.SetActive(true);
        infoGreed.SetActive(true);
    }

    public void TriangleFalse()
    {
        infoSand.SetActive(false);
        infoNag.SetActive(false);
        infoEmr.SetActive(false);
        infoGreed.SetActive(false);
    }


    public void SetSkin()
    {
        P1SkinSelect = P1Char.GetComponent<PlayerBody>().playerSkin;
    }
    public int GetSkin()
    {
        return P1SkinSelect;
    }

    public void SetChar()
    {
        P1CharSelect = P1Char.GetComponent<PlayerBody>().getCharacter();
    }

    public int GetChar()
    {
        return P1CharSelect;
    }

    public void Confirmed()
    {
        confirmed = true;
    }
}
