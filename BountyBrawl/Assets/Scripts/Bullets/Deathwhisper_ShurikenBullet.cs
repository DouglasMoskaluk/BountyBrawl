using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deathwhisper_ShurikenBullet : MonoBehaviour
{
    [SerializeField] private float speed = 50f; //Speed of bullet
    [SerializeField] private float returnSpeed = 100f; //Speed of bullet
    [SerializeField] private float lifeTime = 8f; //Life until it dies
    private Rigidbody2D bulletGO;

    private Vector3 direction;

    private float tempLifeTime;

    private LineRenderer lineRenderer;

    private PlayerBody player; //The player that shot the weapon

    private BoxCollider2D damageArea;

    //Weaponry
    [SerializeField] private float rotationSpeed = 1000f;
    [SerializeField] private float baseDamage = 10f; //The base damage of the weapon without poison
    [SerializeField] private float baseReturnDMG = 5f; //The normal return damage for characters without proficiency
    [SerializeField] private Gradient profGradient;
    [SerializeField] private Gradient nonProfGradient;

    private bool returning; //If the shuriken is returning
    private bool stuck; //If the shuriken is stuck on a wall

    [SerializeField] private AudioSource throwed;
    [SerializeField] private AudioSource hit;
    [SerializeField] private AudioSource returnSound;

    private void Awake()
    {
        tempLifeTime = lifeTime;
        damageArea = GetComponent<BoxCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        damageArea.enabled = true;
        stuck = false;
        returning = false;
        lifeTime = tempLifeTime;
        throwed.Play();
    }

    private void OnDisable()
    {
        throwed.Stop();
    }

    private void Start()
    {
        bulletGO = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, player.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if((lifeTime -= Time.deltaTime) < 0 && !returning && !stuck)
        {
            gameObject.SetActive(false);
        }

        if (returning)
        {
            Vector2 traj = player.transform.position - transform.position; //Get the trajectory of the bullet
            traj.Normalize();

            transform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime));
            bulletGO.velocity = traj * returnSpeed; //Moves gameobject using rigidbody
        }
        else
        {
            if (!stuck)
            {
                transform.Rotate(new Vector3(0f, 0f, -rotationSpeed * Time.deltaTime));
                bulletGO.velocity = direction.normalized * speed; //Moves gameobject using rigidbody
            }
        }


    }

    public void Fire(Vector3 dir, PlayerBody play)
    {
        direction = dir; //assigns given direction to direction variable.
        player = play; //If the player is profficient

        if(player.getCharacter() == 1)
        {
            lineRenderer.colorGradient = profGradient;
        }
        else
        {
            lineRenderer.colorGradient = nonProfGradient;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" && !returning)
        {
            bulletGO.velocity = Vector2.zero;
            throwed.Stop();
            hit.Play();
            stuck = true;
        }
        else if (collision.gameObject != player.gameObject && collision.transform.tag == "Player")
        {
            PlayerBody enemy = collision.GetComponent<PlayerBody>();

            if (returning)
            {
                if (player.getCharacter() == 1)
                {
                    enemy.damagePlayer(baseDamage, player);
                }
                else
                {
                    enemy.damagePlayer(baseReturnDMG, player);
                }
            }
            else
            {
                enemy.damagePlayer(baseDamage, player);
            }

        }
        else if(collision.transform.tag == "Lost")
        {
            TheLost enemy = collision.GetComponent<TheLost>();

            if (returning)
            {
                if (player.getCharacter() == 1)
                {
                    enemy.DamageEnemy(baseDamage, player);
                }
                else
                {
                    enemy.DamageEnemy(baseReturnDMG, player);
                }
            }
            else
            {
                enemy.DamageEnemy(baseDamage, player);
            }

        }
        else if (collision.transform.tag == "Eater")
        {
            TheEater enemy = collision.GetComponent<TheEater>();

            if (returning)
            {
                if(player.getCharacter() == 1)
                {
                    enemy.DamageEnemy(baseDamage, player);
                }
                else
                {
                    enemy.DamageEnemy(baseReturnDMG, player);
                }
            }
            else
            {
                enemy.DamageEnemy(baseDamage, player);
            }
            //If player is proficient
        }
        else if (collision.tag == "Box")
        {
            WeaponBox box = collision.GetComponent<WeaponBox>();
            player.IncreaseMoney(box.GetMoney());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (returning && other.transform.tag == "Player" && other.gameObject == player.gameObject)
        {
            gameObject.SetActive(false);
        }
    }

    public void ReturnToPlayer()
    {
        if (!returning)
        {
            returnSound.Play();
        }
        stuck = false;
        returning = true;
        bulletGO.velocity = Vector2.zero;
    }

    public void Thrown()
    {
        gameObject.SetActive(false);
    }
}
