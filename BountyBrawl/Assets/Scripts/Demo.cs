using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Demo : MonoBehaviour
{
    [SerializeField] private GameObject demoWeaponBox;
    [SerializeField] private GameObject demoWeapons;
    [SerializeField] private GameObject demoEnemies;
    [SerializeField] private GameObject demoCluster;
    [SerializeField] private GameObject eventManager;
    [SerializeField] private GameObject eater;

    private Transform[] losts;

    private void Awake()
    {

        losts = demoCluster.GetComponentsInChildren<Transform>();

        demoCluster.SetActive(false);

    }
    // Update is called once per frame
    void Update()
    {

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
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
        else if (Keyboard.current.digit4Key.wasPressedThisFrame)
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
        else if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            if (demoCluster.activeSelf)
            {
                demoCluster.SetActive(false);
            }
            else
            {
                demoCluster.SetActive(true);

                foreach(Transform l in losts)
                {
                    l.gameObject.SetActive(true);
                }
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
        }else if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            if (eventManager.activeSelf)
            {
                eventManager.SetActive(false);
            }
            else
            {
                eventManager.SetActive(true);
                eventManager.GetComponent<EventManager>().Quicken();
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
                StartCoroutine(eater.GetComponent<TheEater>().ChangeSpawnin());
            }
        }else if (Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            eater.GetComponent<TheEater>().IsNotTeleporting(true);
            StartCoroutine(IsMiniboss());
        }
    }

    private IEnumerator IsMiniboss()
    {
        yield return new WaitForSeconds(10);
        eater.GetComponent<TheEater>().IsMiniboss();
    }
}
