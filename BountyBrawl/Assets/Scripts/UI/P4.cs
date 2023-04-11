using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class P4 : MonoBehaviour
{

    GameObject P4Char;

    bool confirmed;

    [SerializeField]
    GameObject infoSand, infoNag, infoEmr, infoGreed;

    int P4CharSelect;
    int P4SkinSelect;

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
        P4Char = GameObject.FindGameObjectWithTag("P4 Char");
      

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
        P4SkinSelect = P4Char.GetComponent<PlayerBody>().playerSkin;
    }
    public int GetSkin()
    {
        return P4SkinSelect;
    }

    public void SetChar()
    {
        P4CharSelect = P4Char.GetComponent<PlayerBody>().getCharacter();
    }

    public int GetChar()
    {
        return P4CharSelect;
    }

    public void Confirmed()
    {
        confirmed = true;
    }
}
