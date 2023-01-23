using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingScreen : MonoBehaviour
{

    public int randNum;
    public int randTime;
    public TMP_Text tipsText;

    void OnEnable()
    {
        randTime = Random.Range(2, 8);
        TipSelect();
    }



    public void TipSelect()
    {
        
        randNum = Random.Range(1, 4);
        if (randNum == 1)
        {
            tipsText.text = "I ran out of M&Ms while coding this";
        }
        if (randNum == 2)
        {
            tipsText.text = "Its 2:32AM rn";
        }
        if (randNum == 3)
        {
            tipsText.text = "Shout out to GameDevTraum Uploads on youtube for showing me how to use TextMesh Pro UI and changing it via scripts";
        }
    }   
}
