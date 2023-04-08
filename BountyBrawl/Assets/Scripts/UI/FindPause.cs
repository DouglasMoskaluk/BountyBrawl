using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPause : MonoBehaviour
{
    [SerializeField]
    GameObject UIPauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        UIPauseMenu = GameObject.Find("PauseMenu"); //finds pause menu ui
    }



    public GameObject GetPauseMenu()
    {
        return UIPauseMenu;
    }
}
