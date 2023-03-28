using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Demo : MonoBehaviour
{
    [SerializeField] private GameObject demoWeaponBox;
    [SerializeField] private GameObject demoWeapons;
    [SerializeField] private GameObject demoEnemies;
    [SerializeField] private GameObject eventManager;
    private GameObject eater;

    // Update is called once per frame
    void Update()
    {
        
        if(FindObjectOfType<TheEater>() != null && eater == null)
        {
            eater = GameObject.FindGameObjectWithTag("Eater");
        }


        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            if (demoWeaponBox.activeSelf)
            {
                demoWeaponBox.SetActive(false);
            }
            else
            {
                demoWeaponBox.SetActive(true);
            }
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            if (demoWeapons.activeSelf)
            {
                demoWeapons.SetActive(false);
            }
            else
            {
                demoWeapons.SetActive(true);
            }
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            if (demoEnemies.activeSelf)
            {
                demoEnemies.SetActive(false);
            }
            else
            {
                demoEnemies.SetActive(true);
            }
        }
        else if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            PlayerBody[] players = FindObjectsOfType<PlayerBody>();

            foreach (PlayerBody p in players)
            {
                if (p.playerSkin < 3)
                {
                    p.playerSkin++;
                    p.ChangeSkin();
                }
                else
                {
                    p.playerSkin = 0;
                    p.ChangeSkin();
                }
            }
        }else if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            if (eventManager.activeSelf)
            {
                eventManager.SetActive(false);
            }
            else
            {
                eventManager.SetActive(true);
            }
        }
        else if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            if (eater.activeSelf)
            {
                eater.SetActive(false);
            }
            else
            {
                eater.SetActive(true);
            }
        }else if (Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            eater.GetComponent<TheEater>().IsMiniboss();
        }
    }
}
