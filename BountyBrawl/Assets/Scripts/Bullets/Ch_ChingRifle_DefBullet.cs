using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch_ChingRifle_DefBullet : MonoBehaviour
{
    [SerializeField] private float speed = 50f; //Speed of bullet
    [SerializeField] private float lifeTime = 8f; //Life until it dies
    private Rigidbody2D bulletGO;

    private Vector3 direction;

    private float tempLifeTime;

    private Vector3 spawnPos;

    private PlayerBody player; //The player that shot the weapon

    //Weaponry
    [SerializeField] private float baseDamage = 10f; //The base damage of the default bullet


    private void Awake()
    {
        tempLifeTime = lifeTime;
    }

    private void OnEnable()
    {
        lifeTime = tempLifeTime;
    }

    private void Start()
    {
        bulletGO = gameObject.GetComponent<Rigidbody2D>();
        spawnPos = transform.position;
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
        }
        else if (collision.gameObject != player.gameObject && collision.transform.tag == "Player")
        {
            PlayerBody enemy = collision.GetComponent<PlayerBody>();

            //Damage player by base
            enemy.damagePlayer(baseDamage, player);
            gameObject.SetActive(false);


        }
        else if(collision.transform.tag == "Lost")
        {
            TheLost enemy = collision.GetComponent<TheLost>();
            enemy.DamageEnemy(baseDamage, player);
            gameObject.SetActive(false);

        }
        else if (collision.transform.tag == "Eater")
        {
            TheEater enemy = collision.GetComponent<TheEater>();

            enemy.DamageEnemy(baseDamage, player);
            gameObject.SetActive(false);
        }
    }
}
