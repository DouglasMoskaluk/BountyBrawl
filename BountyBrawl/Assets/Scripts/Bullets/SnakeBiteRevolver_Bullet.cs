using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBiteRevolver_Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 50f; //Speed of bullet
    [SerializeField] private float lifeTime = 8f; //Life until it dies
    private Rigidbody2D bulletGO;

    private Vector3 direction;

    private float tempLifeTime;

    private Vector3 spawnPos;

    private PlayerBody player; //The player that shot the weapon

    //Weaponry
    [SerializeField] private float baseDamage = 10f; //The base damage of the weapon without poison
    [SerializeField] private float poisonDistance = 5f; //The distance the player has to be inorder to be poisoned
    [SerializeField] private float poisonDamage = 5f; //The damage a player takes each time
    [SerializeField] private float poisonInterval = 3f; //How often the player is damaged by poison
    [SerializeField] private float poisonAmount = 3f; //How many times the poisoned player is damaged


    private void Awake()
    {
        tempLifeTime = lifeTime;
    }

    private void OnEnable()
    {
        lifeTime = tempLifeTime;
        spawnPos = this.transform.position;
    }

    private void Start()
    {
        bulletGO = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.right = direction;
        bulletGO.velocity = transform.right * speed; //Moves gameobject using rigidbody

        if((lifeTime -= Time.deltaTime) < 0)
        {
            gameObject.SetActive(false);
        }


    }

    public void Fire(Vector3 dir, PlayerBody play)
    {
        direction = dir; //assigns given direction to direction variable.
        player = play; //If the player is profficient
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            gameObject.SetActive(false);
        }else if (collision.gameObject != player.gameObject && collision.transform.tag == "Player")
        {
            PlayerBody enemy = collision.GetComponent<PlayerBody>();

            //Damage player by base
            enemy.damagePlayer(baseDamage);

            //If player is proficient
            if (player.GetPlayerCharacter() == 0)
            {
                enemy.StartCoroutine(enemy.Poison(poisonDamage, poisonInterval, poisonAmount));
                gameObject.SetActive(false);
            }
            else
            {
                if(Vector3.Distance(enemy.transform.position, spawnPos) <= poisonDistance){
                    enemy.StartCoroutine(enemy.Poison(poisonDamage, poisonInterval, poisonAmount));
                }
                gameObject.SetActive(false);
            }

        }
    }
}
