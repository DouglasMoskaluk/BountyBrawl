using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeColour : MonoBehaviour
{
    [SerializeField]
    InputActionReference square;
    GameObject P1Char;

    bool squarePressed;

    public void Start()
    {
        //P1Char = GameObject.FindGameObjectWithTag("P1 Char");
        
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

    }

    private void OnEnable()
    {


        square.action.performed += SwapSkin;


    }

    private void SwapSkin(InputAction.CallbackContext obj)
    {
        squarePressed = true;
    }

    private void OnDisable()
    {
        square.action.performed -= SwapSkin;
    }
}
