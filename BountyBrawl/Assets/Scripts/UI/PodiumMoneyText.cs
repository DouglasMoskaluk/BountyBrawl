using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PodiumMoneyText : MonoBehaviour
{
    [SerializeField]
    int index;


    public TMP_Text MoneyText;
    void Start()
    {
       // index = GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().
    }

    public void WinnerMoney()
    {


        if (index == GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().Get4th())
        {
            MoneyText.text = "$6";
        }
        if (index == GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().Get3rd())
        {
            MoneyText.text = "$46,000";
        }
        if (index == GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().Get2nd())
        {
            MoneyText.text = "$275,000";
        }
        else
        {
            MoneyText.text = "$1,849,000";
        }
    }
}
