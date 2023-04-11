using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class P3 : MonoBehaviour
{

    GameObject P3Char;

    bool confirmed;

    [SerializeField]
    GameObject infoSand, infoNag, infoEmr, infoGreed;

    int P3CharSelect;
    int P3SkinSelect;

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
        P3Char = GameObject.FindGameObjectWithTag("P3 Char");


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
        P3SkinSelect = P3Char.GetComponent<PlayerBody>().playerSkin;
    }
    public int GetSkin()
    {
        return P3SkinSelect;
    }

    public void SetChar()
    {
        P3CharSelect = P3Char.GetComponent<PlayerBody>().getCharacter();
    }

    public int GetChar()
    {
        return P3CharSelect;
    }

    public void Confirmed()
    {
        confirmed = true;
    }
}
