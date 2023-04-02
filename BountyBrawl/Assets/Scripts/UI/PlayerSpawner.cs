using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{

    public GameObject sand, nag, emr, greed;
    private GameObject P1, P2, P3, P4;
    private PlayerInput playerInput;
    private int P1Char, P2Char, P3Char, P4Char;
    private int P1Skin, P2Skin, P3Skin, P4Skin;
    private GameObject charToSpawn;
    private GameObject P1Inst, P2Inst, P3Inst, P4Inst;
    private int numDead;
    private bool P1Dead, P2Dead, P3Dead, P4Dead;
    private int players;


    // Start is called before the first frame update
    void Start()
    {
        players = GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().GetPlayers();
        numDead = 0;
        P1Dead = false;
        P2Dead = false;
        P3Dead = false;
        P4Dead = false;

        P1 = GameObject.FindGameObjectWithTag("P1");
        P1Char = P1.GetComponent<P1>().GetChar();
        if (P1Char == 0) { charToSpawn = sand; }
        if (P1Char == 1) { charToSpawn = nag; }
        if (P1Char == 2) { charToSpawn = greed; }
        if (P1Char == 3) { charToSpawn = emr; }
        P1Skin = P1.GetComponent<P1>().GetSkin();
        P1Inst = Instantiate(charToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
        P1Inst.GetComponent<PlayerBody>().playerSkin = P1Skin;
        P1Inst.GetComponent<PlayerBody>().ChangeSkin();
        P1Inst.GetComponent<PlayerBody>().SetPlayerIndex(0);
        P1.GetComponent<InputHandler>().FindPlayer();

        if (players >= 2)
        {
            P2 = GameObject.FindGameObjectWithTag("P2");
            P2Char = P2.GetComponent<P2>().GetChar();
            if (P2Char == 0) { charToSpawn = sand; }
            if (P2Char == 1) { charToSpawn = nag; }
            if (P2Char == 2) { charToSpawn = greed; }
            if (P2Char == 3) { charToSpawn = emr; }
            P2Skin = P2.GetComponent<P2>().GetSkin();
            P2Inst = Instantiate(charToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
            P2Inst.GetComponent<PlayerBody>().playerSkin = P2Skin;
            P2Inst.GetComponent<PlayerBody>().ChangeSkin();
            P2Inst.GetComponent<PlayerBody>().SetPlayerIndex(1);
            P2.GetComponent<InputHandler>().FindPlayer();
        }

        if (players >= 3)
        {
            P3 = GameObject.FindGameObjectWithTag("P3");
            P3Char = P3.GetComponent<P3>().GetChar();
            if (P3Char == 0) { charToSpawn = sand; }
            if (P3Char == 1) { charToSpawn = nag; }
            if (P3Char == 2) { charToSpawn = greed; }
            if (P3Char == 3) { charToSpawn = emr; }
            P3Skin = P3.GetComponent<P3>().GetSkin();
            P3Inst = Instantiate(charToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
            P3Inst.GetComponent<PlayerBody>().playerSkin = P3Skin;
            P3Inst.GetComponent<PlayerBody>().ChangeSkin();
            P3Inst.GetComponent<PlayerBody>().SetPlayerIndex(2);
            P3.GetComponent<InputHandler>().FindPlayer();
        }

        if (players >= 4)
        {
            P4 = GameObject.FindGameObjectWithTag("P4");
            P4Char = P4.GetComponent<P4>().GetChar();
            if (P4Char == 0) { charToSpawn = sand; }
            if (P4Char == 1) { charToSpawn = nag; }
            if (P4Char == 2) { charToSpawn = greed; }
            if (P4Char == 3) { charToSpawn = emr; }
            P4Skin = P4.GetComponent<P4>().GetSkin();
            P4Inst = Instantiate(charToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
            P4Inst.GetComponent<PlayerBody>().playerSkin = P4Skin;
            P4Inst.GetComponent<PlayerBody>().ChangeSkin();
            P4Inst.GetComponent<PlayerBody>().SetPlayerIndex(3);
            P4.GetComponent<InputHandler>().FindPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (P1Inst.GetComponent<PlayerBody>().getDead() == true && P1Dead == false)
        {
            numDead++;
            P1Dead = true;
        }
        if (P2Inst.GetComponent<PlayerBody>().getDead() == true && P2Dead == false)
        {
            numDead++;
            P2Dead = true;
        }
        if (P3Inst.GetComponent<PlayerBody>().getDead() == true && P3Dead == false)
        {
            numDead++;
            P3Dead = true;
        }
        if (P4Inst.GetComponent<PlayerBody>().getDead() == true && P4Dead == false)
        {
            numDead++;
            P4Dead = true;
        }

        if(numDead >= GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().GetPlayers())
        {
            GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().Podium();
        }
    }
}
