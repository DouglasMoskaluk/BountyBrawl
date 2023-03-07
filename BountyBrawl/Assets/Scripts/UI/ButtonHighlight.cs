using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour, ISelectHandler
{
    [SerializeField]
    GameObject MeleeActive;
    [SerializeField]
    GameObject ProfActive;
    [SerializeField]
    GameObject MeleeDeact;
    [SerializeField]
    GameObject ProfDeact;
    [SerializeField]
    GameObject MeleeDeact2;
    [SerializeField]
    GameObject ProfDeact2;
    [SerializeField]
    GameObject MeleeDeact3;
    [SerializeField]
    GameObject ProfDeact3;

    public void OnSelect(BaseEventData eventData)
    {
        MeleeActive.SetActive(true);
        ProfActive.SetActive(true);
        MeleeDeact.SetActive(false);
        MeleeDeact2.SetActive(false);
        MeleeDeact3.SetActive(false);
        ProfDeact.SetActive(false);
        ProfDeact2.SetActive(false);
        ProfDeact3.SetActive(false);
    }

}
