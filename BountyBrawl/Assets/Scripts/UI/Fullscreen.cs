using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fullscreen : MonoBehaviour
{
    [SerializeField]
    GameObject checkMark;
   
    public void FullscreenToggle()
    {
        if (checkMark.activeSelf == false)
        {
            checkMark.SetActive(true);
            Screen.fullScreen = true;
        }
        else
        {
            checkMark.SetActive(false);
            Screen.fullScreen = false;
        }
        
    }


}
