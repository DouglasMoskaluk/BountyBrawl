using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour
{
    [Tooltip("These will hold all the enemy spawn points")] 
    [SerializeField] private Transform[] enemySP; //Array of all the enemySpawnPoints

    [Tooltip("The enemy to be spawned")]
    [SerializeField] private GameObject lost; //The lost to be spawned

    [Tooltip("The enemy to be spawned")]
    [SerializeField] private GameObject eater; //The enemy to be spawned

    [SerializeField] private float numEnemies = 5f;

    [Tooltip ("The amount of time needed until the enemies spawn")]
    [SerializeField] private float enemyTimer = 60f;

    [Tooltip("The amount of time needed until the weapon box spawn")]
    [SerializeField] private float boxTimer = 60f; //Used later when weapons added

    [SerializeField] private TextMeshProUGUI timer;

    [Tooltip("The increase of damage for enemies each time they are spawned")]
    [SerializeField] private float enemyDamageIncrease = 4f;

    [Tooltip("The increase of damage for enemies each time they are spawned")]
    [SerializeField] private float enemyHealthIncrease = 10f;

    [Tooltip("The number of times events until the Eater becomes a miniboss")]
    [SerializeField] private int miniboss = 4;

    private int numSpawn = 0; //The number of times a group of enemies have spawned

    private int tempSp;

    private bool canSpawn;

    TheEater currEater;

    private float tempTimer;

    private void Awake()
    {
        tempTimer = enemyTimer;
        canSpawn = true;
        tempSp = 0;
    }

    private void Update()
    {
        if(enemyTimer > 0)
        {
            enemyTimer -= Time.deltaTime;
            timer.text = System.Convert.ToString((int)enemyTimer);
        }
        else
        {
            enemyTimer = tempTimer;
            SpawnEnemies();

            if(numSpawn >= miniboss) //If the eater will become a miniboss
            {
                currEater.IsMiniboss();
            }
            else
            {
                Destroy(currEater.gameObject); //Turn off eater
            }
        }

        if (Mathf.Round(enemyTimer) <= 10 && canSpawn) //Spawns the eater at the enemy spawn position
        {
            canSpawn = false;
            tempSp = (int)Random.Range(0f, enemySP.Length - 1);
            currEater = Instantiate(eater, enemySP[tempSp].position, Quaternion.identity).GetComponent<TheEater>();
            currEater.gameObject.SetActive(true);  //Turn on eater
        }
    }


    private void SpawnEnemies()
    {

        //Spawns the number of enemies as declared as numEnemies
        for(int i = 0; i <= numEnemies - 1 + numSpawn; i++)
        {
            //gets the position for enemy spawn and makes sure enemies aren't stuck on eachother
            Vector3 spawn = new Vector3(enemySP[tempSp].position.x + i/1.2f, enemySP[tempSp].position.y - i/ 1.2f, 0f);
            TheLost minion = Instantiate(lost, spawn, Quaternion.identity).GetComponent<TheLost>();

            //Increases the losts damage and health each time they are spawned
            minion.AddDamage(enemyDamageIncrease * numSpawn);
            minion.AddHealth(enemyHealthIncrease * numSpawn);
            minion.gameObject.SetActive(true);
        }
        canSpawn = true; //Can spawn the eater for the next event
        numSpawn++; //Adds 1 to number of spawns each time enemies are spawned
    }
}
