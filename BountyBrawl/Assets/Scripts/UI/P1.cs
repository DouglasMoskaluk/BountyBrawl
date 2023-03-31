using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class P1 : MonoBehaviour
{
    [SerializeField]
    InputActionReference square, triangle;
    GameObject P1Char;
    bool squarePressed;
    bool trianglePressed;

    [SerializeField]
    GameObject infoSand, infoNag, infoEmr, infoGreed;

    public void Start()
    {
        squarePressed = false;
        

    }
    // Update is called once per frame
    void Update()
    {
        P1Char = GameObject.FindGameObjectWithTag("P1 Char");
        if (squarePressed == true)
        {
            if (P1Char.GetComponent<PlayerBody>().playerSkin < 3)
            {
                squarePressed = false;
                P1Char.GetComponent<PlayerBody>().playerSkin++;
                P1Char.GetComponent<PlayerBody>().ChangeSkin();
            }
            else
            {
                P1Char.GetComponent<PlayerBody>().playerSkin = 0;
                P1Char.GetComponent<PlayerBody>().ChangeSkin();
                squarePressed = false;
            }
        }
        
        if(trianglePressed == true)
        {
            infoSand.SetActive(true);
            infoNag.SetActive(true);
            infoEmr.SetActive(true);
            infoGreed.SetActive(true);

            
        }
        if(trianglePressed == false)
        {
            infoSand.SetActive(false);
            infoNag.SetActive(false);
            infoEmr.SetActive(false);
            infoGreed.SetActive(false);
        }

    }

    private void OnEnable()
    {
        square.action.performed += SwapSkin;
        triangle.action.started += ShowInfo;
        triangle.action.canceled += HideInfo;
    }

    private void HideInfo(InputAction.CallbackContext obj)
    {

        Debug.Log("triangle released");
        trianglePressed = false;
    }

    private void ShowInfo(InputAction.CallbackContext obj)
    {
        Debug.Log("triangle");
        trianglePressed = true;
        

    }

    private void SwapSkin(InputAction.CallbackContext obj)
    {
        Debug.Log("square");
        squarePressed = true;
    }

    private void OnDisable()
    {
        square.action.performed -= SwapSkin;
        triangle.action.started -= ShowInfo;
        triangle.action.canceled -= HideInfo;
    }
}
