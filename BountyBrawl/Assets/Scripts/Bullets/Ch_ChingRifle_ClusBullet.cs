using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ch_ChingRifle_ClusBullet : MonoBehaviour
{
    [SerializeField] private float speed = 50f; //Speed of bullet
    private Rigidbody2D bulletGO;

    private Vector3 direction;

    private float silvertempLifeTime;
    private float goldtempLifeTime;

    private Vector3 spawnPos;

    private PlayerBody player; //The player that shot the weapon

    //Weaponry
    [SerializeField] private float silverDamage = 5f; //The damage of the silver bullets
    [SerializeField] private float goldDamage = 10f; //The damage of the gold bullets
    [SerializeField] private float silverLifeTime = 0.5f; //Life until silver cluster dies
    [SerializeField] private float goldLifeTime = 2f; //Life until gold cluster dies

    [SerializeField] private Sprite silverCluster;
    [SerializeField] private Sprite goldCluster;

    [SerializeField] private Gradient silverBulletTrail;
    [SerializeField] private Gradient goldBulletTrail;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        silvertempLifeTime = silverLifeTime;
        goldtempLifeTime = goldLifeTime;
        trailRenderer = GetComponent<TrailRenderer>();

    }

    private void OnEnable()
    {
        silverLifeTime = silvertempLifeTime;
        goldLifeTime = goldtempLifeTime;
    }

    private void Start()
    {
        bulletGO = gameObject.GetComponent<Rigidbody2D>();
        spawnPos = transform.position;

        if (player.getCharacter() == 2)
        {
            spriteRenderer.sprite = goldCluster;
            trailRenderer.colorGradient = goldBulletTrail;
        }
        else
        {
            spriteRenderer.sprite = silverCluster;
            trailRenderer.colorGradient = silverBulletTrail;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.right = direction;
        bulletGO.velocity = transform.right * speed; //Moves gameobject using rigidbody

        if (player.getCharacter() == 2)
        {
            if ((goldLifeTime -= Time.deltaTime) < 0)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if ((silverLifeTime -= Time.deltaTime) < 0)
            {
                gameObject.SetActive(false);
            }
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
            if (player.getCharacter() == 2)
            {
                enemy.damagePlayer(goldDamage);
            }
            else
            {
                enemy.damagePlayer(silverDamage);
            }

            gameObject.SetActive(false);

        }
        else if(collision.transform.tag == "Lost")
        {
            TheLost enemy = collision.GetComponent<TheLost>();
            if (player.getCharacter() == 2)
            {
                enemy.DamageEnemy(goldDamage);
            }
            else
            {
                enemy.DamageEnemy(silverDamage);
            }

            gameObject.SetActive(false);

        }
        else if (collision.transform.tag == "Eater")
        {
            TheEater enemy = collision.GetComponent<TheEater>();

            if (player.getCharacter() == 2)
            {
                enemy.DamageEnemy(goldDamage);
            }
            else
            {
                enemy.DamageEnemy(silverDamage);
            }

            gameObject.SetActive(false);
        }
    }
}
