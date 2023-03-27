using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    GameObject ActiveChar;
    [SerializeField]
    GameObject NonActiveChar1;
    [SerializeField]
    GameObject NonActiveChar2;
    [SerializeField]
    GameObject NonActiveChar3;


    public void OnSelect(BaseEventData eventData)
    {
        ActiveChar.SetActive(true);
        NonActiveChar1.SetActive(false);
        NonActiveChar2.SetActive(false);
        NonActiveChar3.SetActive(false);
    }

}
