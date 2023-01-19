using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEater : MonoBehaviour
{

    private PlayerBody[] players;
    private float distance;
    private GameObject target;

    [SerializeField] private GameObject minion;

    [SerializeField] private int numMinions = 4;

    [SerializeField] private float enemySpeed = 4f;

    [SerializeField] private float baseDamage = 20f;

    [SerializeField] private float baseHealth = 500f;

    [SerializeField] private float minionSpawn = 30f;

    private bool isMiniboss = false;

    private float currDamage;
    private float currHealth;

    private float tempTimer;

    private Rigidbody2D rb;

    private void OnEnable()
    {
        players = FindObjectsOfType<PlayerBody>();
        rb = GetComponent<Rigidbody2D>();
        isMiniboss = false;
        currHealth = baseHealth;
        currDamage = baseDamage;
        tempTimer = minionSpawn;
    }


    void Update()
    {
        if (isMiniboss)
        {

            if (minionSpawn > 0)
            {
                minionSpawn -= Time.deltaTime;
            }
            else
            {
                minionSpawn = tempTimer;
                SpawnMinions();
            }

            distance = float.PositiveInfinity;

            foreach (var player in players)
            {
                float dist = Vector3.Distance(player.transform.position, transform.position);

                if (dist < distance)
                {
                    distance = dist;
                    target = player.gameObject;
                }
            }

            if (currHealth <= 0f)
            {
                Death();
            }

        }
    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.Normalize();
            rb.MovePosition(transform.position + (direction * enemySpeed * Time.deltaTime));
        }
    }

    private void LateUpdate()
    {

        if (target != null)
        {
            Vector2 face = target.transform.position - transform.position; //Get 2d position of the player

            if (face.x > 0.1)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

    }

    private void SpawnMinions()
    {
        //Spawns the number of minions as declared as numMinions
        for (int i = 0; i <= numMinions - 1; i++)
        {
            //gets the position for enemy spawn and makes sure enemies aren't stuck on eachother
            Vector3 spawn = new Vector3(transform.position.x + i / 1.2f, transform.position.y - i / 1.2f, 0f);
            TheLost lost = Instantiate(minion, spawn, Quaternion.identity).GetComponent<TheLost>();

            lost.gameObject.SetActive(true);
        }
    }

    public void IsMiniboss(){isMiniboss = true;} //If the eater is now a miniboss

    private void Death()
    {
        Destroy(gameObject);
    }
}
