using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventManager : MonoBehaviour
{
    [Tooltip("These will hold all the enemy spawn points")] 
    [SerializeField] private Transform[] enemySP; //Array of all the enemySpawnPoints

    [Tooltip("The enemy to be spawned")]
    [SerializeField] private GameObject enemy; //The enemy to be spawned

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

    private int numSpawn = 0; //The number of times a group of enemies have spawned


    private float tempTimer;

    private void Awake()
    {
        tempTimer = enemyTimer;
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
        }
    }


    private void SpawnEnemies()
    {
        int ranSpawn = (int) Random.Range(0f, enemySP.Length-1);

        //Spawns the number of enemies as declared as numEnemies
        for(int i = 0; i <= numEnemies - 1; i++)
        {
            //gets the position for enemy spawn and makes sure enemies aren't stuck on eachother
            Vector3 spawn = new Vector3(enemySP[ranSpawn].position.x + i/1.2f, enemySP[ranSpawn].position.y - i/ 1.2f, 0f);
            Enemy lost = Instantiate(enemy, spawn, Quaternion.identity).GetComponent<Enemy>();

            lost.AddDamage(enemyDamageIncrease * numSpawn);
            lost.AddHealth(enemyHealthIncrease * numSpawn);
            //enemy.GetComponent<Enemy>().AddDamage(enemyDamageIncrease * numSpawn);
            //enemy.GetComponent<Enemy>().AddHealth(enemyHealthIncrease * numSpawn);
            enemy.SetActive(true);
        }

        numSpawn++; //Adds 1 to number of spawns each time enemies are spawned
    }
}
