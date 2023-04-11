using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BustPicker : MonoBehaviour
{
    private int skin;
    private int character;

    [SerializeField]
    int poster;

    Sprite newImage;

    [SerializeField]
    Image baseImage;

    [SerializeField]
    Sprite Nag1, Nag2, Nag3, Nag4, Sand1, Sand2, Sand3, Sand4, Greed1, Greed2, Greed3, Greed4, Emr1, Emr2, Emr3, Emr4;
   
    void Start()
    {

        if(poster == 1)
        {
            character = GameObject.Find("MultiEventSystemP1").GetComponent<P1>().GetChar();
            skin = GameObject.Find("MultiEventSystemP1").GetComponent<P1>().GetSkin();
            FindCombo();
            baseImage.sprite = newImage;
        }
        if (poster == 2)
        {
            character = GameObject.Find("MultiEventSystemP2").GetComponent<P2>().GetChar();
            skin = GameObject.Find("MultiEventSystemP2").GetComponent<P2>().GetSkin();
            FindCombo();
            baseImage.sprite = newImage;
        }
        if (poster == 3)
        {
            character = GameObject.Find("MultiEventSystemP3").GetComponent<P3>().GetChar();
            skin = GameObject.Find("MultiEventSystemP3").GetComponent<P3>().GetSkin();
            FindCombo();
            baseImage.sprite = newImage;
        }
        if (poster == 4)
        {
            character = GameObject.Find("MultiEventSystemP4").GetComponent<P4>().GetChar();
            skin = GameObject.Find("MultiEventSystemP4").GetComponent<P4>().GetSkin();
            FindCombo();
            baseImage.sprite = newImage;
        }
    }

    private void FindCombo()
    {
        if(character == 0)
        {
            if(skin == 0)
            {
                newImage = Sand1;
            }
            if (skin == 1)
            {
                newImage = Sand2;
            }
            if (skin == 2)
            {
                newImage = Sand3;
            }
            if (skin == 3)
            {
                newImage = Sand4;
            }
        }
        if (character == 1)
        {
            if (skin == 0)
            {
                newImage = Nag1;
            }
            if (skin == 1)
            {
                newImage = Nag2;
            }
            if (skin == 2)
            {
                newImage = Nag3;
            }
            if (skin == 3)
            {
                newImage = Nag4;
            }
        }
        if (character == 2)
        {
            if (skin == 0)
            {
                newImage = Greed1;
            }
            if (skin == 1)
            {
                newImage = Greed2;
            }
            if (skin == 2)
            {
                newImage = Greed3;
            }
            if (skin == 3)
            {
                newImage = Greed4;
            }
        }
        if (character == 3)
        {
            if (skin == 0)
            {
                newImage = Emr1;
            }
            if (skin == 1)
            {
                newImage = Emr2;
            }
            if (skin == 2)
            {
                newImage = Emr3;
            }
            if (skin == 3)
            {
                newImage = Emr4;
            }
        }
    }
    
}
