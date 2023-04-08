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
    private bool squarePressed;
    private bool trianglePressed;
    bool confirmed;

    [SerializeField]
    GameObject infoSand, infoNag, infoEmr, infoGreed;

    int P1CharSelect;
    int P1SkinSelect;

    public void Awake()
    {
        
        DontDestroyOnLoad(transform.gameObject);
        
    }
    public void Start()
    {
        squarePressed = false;
        confirmed = false;
        

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

        trianglePressed = false;
    }

    private void ShowInfo(InputAction.CallbackContext obj)
    {
        trianglePressed = true;
        

    }

    private void SwapSkin(InputAction.CallbackContext obj)
    {
        if (confirmed == false)
        {
            squarePressed = true;
        }
    }

    private void OnDisable()
    {
        square.action.performed -= SwapSkin;
        triangle.action.started -= ShowInfo;
        triangle.action.canceled -= HideInfo;
    }

    public void SetSkin()
    {
        P1SkinSelect = P1Char.GetComponent<PlayerBody>().playerSkin;
    }
    public int GetSkin()
    {
        return P1SkinSelect;
    }

    public void SetChar()
    {
        P1CharSelect = P1Char.GetComponent<PlayerBody>().getCharacter();
    }

    public int GetChar()
    {
        return P1CharSelect;
    }

    public void Confirmed()
    {
        confirmed = true;
    }
}
