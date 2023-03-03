using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Demo : MonoBehaviour
{
    [SerializeField] private GameObject demoWeaponBox;
    [SerializeField] private GameObject demoWeapons;
    [SerializeField] private GameObject demoEnemies;

    // Update is called once per frame
    void Update()
    {
        
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            demoWeaponBox.SetActive(true);
        }else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            demoWeapons.SetActive(true);
        }else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            demoEnemies.SetActive(true);
        }
    }
}
