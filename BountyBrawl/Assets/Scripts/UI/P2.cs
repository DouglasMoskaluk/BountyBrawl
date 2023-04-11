using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class P2 : MonoBehaviour
{

    GameObject P2Char;

    bool confirmed;

    [SerializeField]
    GameObject infoSand, infoNag, infoEmr, infoGreed;

    int P2CharSelect;
    int P2SkinSelect;

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
        P2Char = GameObject.FindGameObjectWithTag("P2 Char");

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
        P2SkinSelect = P2Char.GetComponent<PlayerBody>().playerSkin;
    }
    public int GetSkin()
    {
        return P2SkinSelect;
    }

    public void SetChar()
    {
        P2CharSelect = P2Char.GetComponent<PlayerBody>().getCharacter();
    }

    public int GetChar()
    {
        return P2CharSelect;
    }

    public void Confirmed()
    {
        confirmed = true;
    }
}
