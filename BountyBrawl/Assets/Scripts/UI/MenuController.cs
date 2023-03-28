using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuController : MonoBehaviour
{

    [SerializeField]
    InputActionReference square, triangle;

    private int skinNum;

    [SerializeField]
    private GameObject regular;
    [SerializeField]
    private GameObject green;
    [SerializeField]
    private GameObject blue;
    [SerializeField]
    private GameObject red;


    private void OnEnable()
    {
        square.action.performed += SwapSkin;
        triangle.action.started += ShowInfo;
    }

    private void ShowInfo(InputAction.CallbackContext obj)
    {
        
    }

    private void SwapSkin(InputAction.CallbackContext obj)
    {
        
        
    }

    private void OnDisable()
    {
        square.action.performed -= SwapSkin;
        triangle.action.started -= ShowInfo;
    }
}
