using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour
{
    [Tooltip("These will hold all the enemy spawn points")] 
    [SerializeField] private Transform[] enemySP; //Array of all the enemySpawnPoints

    [Tooltip("The enemy to be spawned")]
    [SerializeField] private GameObject eater; //The enemy to be spawned

    [SerializeField] private float numEnemies = 5f;

    [Tooltip ("The amount of time needed until the enemies spawn")]
    [SerializeField] private float enemyTimer = 60f;

    [Tooltip("The amount of time needed until the weapon box spawn")]
    [SerializeField] private float boxTimer = 60f; //Used later when weapons added

    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private GameObject curseTimer;

    [Tooltip("The increase of damage for enemies each time they are spawned")]
    [SerializeField] private float enemyDamageIncrease = 4f;

    [Tooltip("The increase of damage for enemies each time they are spawned")]
    [SerializeField] private float enemyHealthIncrease = 10f;

    [Tooltip("The number of times events until the Eater becomes a miniboss")]
    [SerializeField] private int miniboss = 4;

    [Tooltip("These will hold all the weapon box spawn points")]
    [SerializeField] private WeaponBox[] weaponBoxes; //Array of all the weapon box spawn points

    [SerializeField] private float graphScanUpdate = 1f;

    [SerializeField] private float cursedTime = 45;

    private int numSpawn = 0; //The number of times a group of enemies have spawned

    private int tempSp;

    private bool canSpawn;
    private bool teleport;

    TheEater currEater;
    TheEater teleportEater; //The eater when teleporting

    private float tempTimer;
    private float tempBoxTimer;

    private bool minibossInUse; //If the eater is a miniboss stop spawning enemies
    private int checker = 0;

    private AstarPath astar;

    private float tempGraph;
    private bool cursed;

    private void Awake()
    {
        tempTimer = enemyTimer;
        canSpawn = true;
        teleport = true;
        tempSp = 0;
        minibossInUse = false;
        tempBoxTimer = boxTimer;
        astar = FindObjectOfType<AstarPath>();
        tempGraph = graphScanUpdate;
        cursed = false;
    }

    private void Update()
    {
        //Stop spawning enemies when the eater is a miniboss
        if (!minibossInUse)
        {
            //Counter goes down to spawn enemies
            if (enemyTimer > 0)
            {
                enemyTimer -= Time.deltaTime;
                timer.text = System.Convert.ToString((int)enemyTimer);
            }

            //Timer is done so spawn enemies
            else
            {
                enemyTimer = tempTimer;

                if (numSpawn >= miniboss) //If the eater will become a miniboss
                {
                    currEater.gameObject.SetActive(true);
                    currEater.IsMiniboss();
                    minibossInUse = true;
                }
                else
                {
                    SpawnEnemies();
                }
            }
        }

        if(Mathf.Round(enemyTimer) <= 15 && teleport)
        {
            teleport = false;
            tempSp = (int)Random.Range(0f, enemySP.Length - 1);
            StartCoroutine(Teleport());
        }

        if (Mathf.Round(enemyTimer) <= 10 && canSpawn) //Spawns the eater at the enemy spawn position as harbringer
        {
            canSpawn = false;

            StopAllCoroutines();

            currEater = ObjectPooler.Instance.SpawnFromPool("Eater", enemySP[tempSp].position, Quaternion.identity).GetComponent<TheEater>();
            currEater.gameObject.SetActive(true);  //Turn on eater

            //Determines whether to spawn in the white filling bar for when eater will become boss
            if (numSpawn >= miniboss)
            {
                currEater.IsNotTeleporting(true);
            }
            else
            {
                currEater.IsNotTeleporting(false);
            }
        }

        if (boxTimer < 0)
        {
            boxTimer = tempBoxTimer;
            int whichBox = (int)Random.Range(0f, weaponBoxes.Length);

            while (weaponBoxes[whichBox].isActiveAndEnabled)
            {
                checker++;
                whichBox = (int)Random.Range(0f, weaponBoxes.Length);

                if(checker >= 20)
                {
                    checker = 0;
                    boxTimer = tempBoxTimer;
                    break;
                }
            }

            weaponBoxes[whichBox].gameObject.SetActive(true);
        }
        else
        {
            boxTimer -= Time.deltaTime;
        }

        if(graphScanUpdate <= 0)
        {
            astar.Scan();
            graphScanUpdate = tempGraph;
        }
        else
        {
            graphScanUpdate -= Time.deltaTime;
        }

        if (!cursed && cursedTime < 0)
        {
            cursed = true;

            curseTimer.SetActive(false);

            PlayerBody[] players = FindObjectsOfType<PlayerBody>();

            foreach (PlayerBody p in players)
            {
                p.Curse();
            }
        }
        else if(cursedTime > 0)
        { 
            cursedTime -= Time.deltaTime;
            curseTimer.GetComponent<TextMeshProUGUI>().text = System.Convert.ToString((int)cursedTime);
        }
    }


    private void SpawnEnemies()
    {

        if (currEater.SpawnHarbringer())
        {
            currEater.gameObject.SetActive(false);
        }

        canSpawn = true; //Can spawn the eater for the next event
        teleport = true; //Eater can teleport for the next event
        numSpawn++; //Adds 1 to number of spawns each time enemies are spawned
    }

    private IEnumerator Teleport()
    {
        while (canSpawn)
        {
            int teleportLoc = (int)Random.Range(0f, enemySP.Length - 1);
            teleportEater = ObjectPooler.Instance.SpawnFromPool("Eater", enemySP[teleportLoc].position, Quaternion.identity).GetComponent<TheEater>();
            teleportEater.IsTeleporting();
            yield return new WaitForSeconds(0.5f);
            teleportEater.gameObject.SetActive(false);

            yield return new WaitForSeconds(1f);
        }
    }

    public void MinibossDead() { 
        minibossInUse = false;
        canSpawn = true;
        teleport = true;
    }

    public float GetDamageIncrease() { return (numSpawn * enemyDamageIncrease); }
    public float GetHealthIncrease() { return (numSpawn * enemyHealthIncrease); }

    public void IncreaseNumspawn() { numSpawn++; }

}
