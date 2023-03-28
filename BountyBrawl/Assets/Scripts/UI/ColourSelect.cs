using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColourSelect : MonoBehaviour
{

    [SerializeField]
    InputActionReference square, triangle;

    private int skinNum;

    [SerializeField]
    public GameObject regular;
    [SerializeField]
    public GameObject green;
    [SerializeField]
    public GameObject blue;
    [SerializeField]
    public GameObject red;

    [SerializeField]
    public GameObject info;

    public bool confirmed;

    public void Start()
    {
        confirmed = false;
    }

    private void OnEnable()
    {
        
        
            square.action.performed += SwapSkin;
            triangle.action.started += ShowInfo;
            triangle.action.canceled += HideInfo;
        
    }

    private void HideInfo(InputAction.CallbackContext obj)
    {
        info.SetActive(false);
    }

    private void ShowInfo(InputAction.CallbackContext obj)
    {
        info.SetActive(true);
    }

    private void SwapSkin(InputAction.CallbackContext obj)
    {
        if (confirmed == false)
        {
            skinNum++;
            if (skinNum >= 4)
            {
                skinNum = 0;
            }

            if (skinNum == 0)
            {
                regular.SetActive(true);
                green.SetActive(false);
                blue.SetActive(false);
                red.SetActive(false);
            }
            if (skinNum == 1)
            {
                regular.SetActive(false);
                green.SetActive(true);
                blue.SetActive(false);
                red.SetActive(false);
            }
            if (skinNum == 2)
            {
                regular.SetActive(false);
                green.SetActive(false);
                blue.SetActive(true);
                red.SetActive(false);
            }
            if (skinNum == 3)
            {
                regular.SetActive(false);
                green.SetActive(false);
                blue.SetActive(false);
                red.SetActive(true);
            }
            Debug.Log(skinNum);
        }
    }

    private void OnDisable()
    {
        square.action.performed -= SwapSkin;
        triangle.action.started -= ShowInfo;
        triangle.action.canceled -= HideInfo;
    }

    public int GetColour()
    {
        return skinNum;
    }

    public void SetConfirmed()
    {
        confirmed = true;
    }

}


