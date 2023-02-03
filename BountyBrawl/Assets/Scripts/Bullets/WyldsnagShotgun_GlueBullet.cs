using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyldsnagShotgun_GlueBullet : MonoBehaviour
{

    [Tooltip("The amoung of time before the player can be damaged again if player is emerald")]
    [SerializeField] private float damageTickTime = 1.5f;

    [SerializeField] private float speed = 50f; //Speed of bullet
    [SerializeField] private float lifeTime = 8f; //Life until it dies

    [SerializeField] private Sprite bulletSP; //The bullet sprite
    [SerializeField] private Sprite glueSP; //The glue sprite
    [SerializeField] private Sprite acidSP; //The acid sprite for Emerald
    private SpriteRenderer glueRD;

    private Rigidbody2D bulletGO;

    private Vector3 direction;

    private float tempLifeTime;

    private Vector3 spawnPos;

    private PlayerBody player; //The player that shot the weapon

    //Weaponry
    [SerializeField] private float baseDamage = 10f; //The base damage of the weapon without poison
    [SerializeField] private float stunLength = 1f; //The length of stun for players hit
    [SerializeField] private float glueTime = 5f; //The amount of time glue will stay in the level
    [SerializeField] private float glueDamage = 3f; //Amount of damage glue gives if player is emerald
    [SerializeField] private float gluePlayerSlowness = 9f; //How slow the glue will make players
    [SerializeField] private Vector3 glueSize = new Vector3(2f, 2f, 1f);
    [SerializeField] private float glueGrowSpeed = 3f;
    [SerializeField] private float glueLostSlowness = 500f;

    private bool glue;
    private BoxCollider2D bullet; //The bullets hitbox
    private CircleCollider2D glueZone; //The glue hitbox

    private float tempTimer;

    private void Awake()
    {
        glueRD = GetComponent<SpriteRenderer>();
        tempLifeTime = lifeTime;
        bullet = GetComponent<BoxCollider2D>();
        glueZone = GetComponent<CircleCollider2D>();
        tempTimer = damageTickTime;
    }

    private void OnEnable()
    {
        glueRD.enabled = true;
        lifeTime = tempLifeTime;
        glue = false;
        bullet.enabled = true;
        glueZone.enabled = false;

        glueRD.sprite = bulletSP;
    }

    private void Start()
    {
        bulletGO = gameObject.GetComponent<Rigidbody2D>();
        spawnPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!glue)
        {
            transform.right = direction;
            bulletGO.velocity = transform.right * speed; //Moves gameobject using rigidbody
        }
        else
        {
            bulletGO.velocity = Vector2.zero;
        }

        if((lifeTime -= Time.deltaTime) < 0 && !glue)
        {
            gameObject.SetActive(false);
        }

        //Change size of poison area over time
        if (transform.localScale.x < glueSize.x && glue)
        {
            Vector3 temp = transform.localScale;
            temp.x += Time.deltaTime * glueGrowSpeed;
            temp.y += Time.deltaTime * glueGrowSpeed;
            temp.z += Time.deltaTime * glueGrowSpeed;

            transform.localScale = temp;

        }
    }

    public void Fire(Vector3 dir, PlayerBody play)
    {
        direction = dir; //assigns given direction to direction variable.
        player = play; //If the player is profficient
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!glue)
        {
            bullet.enabled = false;
            glueZone.enabled = true;

            if (collision.gameObject.tag == "Wall")
            {
                GetComponent<TrailRenderer>().enabled = false;
                glue = true;
                StartCoroutine(Dissapear());

                //If player is proficient
                if (player.GetPlayerCharacter() == 3)
                {
                    glueRD.sprite = acidSP;
                }
                else
                {
                    glueRD.sprite = glueSP;
                }
            }
            else if (collision.gameObject != player.gameObject && collision.transform.tag == "Player")
            {
                PlayerBody enemy = collision.GetComponent<PlayerBody>();

                //Damage player by base
                enemy.damagePlayer(baseDamage);
                enemy.StartCoroutine(enemy.Stun(stunLength));
                StartCoroutine(Dissapear());

                glue = true;

                //If player is proficient
                if (player.GetPlayerCharacter() == 3)
                {
                    glueRD.sprite = acidSP;
                }
                else
                {
                    glueRD.sprite = glueSP;
                }
            }else if(collision.transform.tag == "Lost")
            {

                TheLost enemy = collision.GetComponent<TheLost>();

                //Damage player by base
                enemy.DamageEnemy(baseDamage);
                enemy.StartCoroutine(enemy.Stun(stunLength));
                StartCoroutine(Dissapear());

                glue = true;

                //If player is proficient
                if (player.GetPlayerCharacter() == 3)
                {
                    glueRD.sprite = acidSP;
                }
                else
                {
                    glueRD.sprite = glueSP;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (glue)
        {
            //If player is in glue other than shot player
            if (collision.gameObject.tag == "Player" && collision.gameObject != player.gameObject)
            {
                collision.GetComponent<PlayerBody>().Slow(gluePlayerSlowness);

                //If player is proficient then make glue deal damage
                if (player.GetPlayerCharacter() == 3)
                {
                    if (damageTickTime > 0)
                    {
                        damageTickTime -= Time.deltaTime;
                    }
                    else
                    {
                        damageTickTime = tempTimer;
                        collision.GetComponent<PlayerBody>().damagePlayer(glueDamage);
                    }
                }
            }
            else if (collision.gameObject.tag == "Lost")
            {
                collision.GetComponent<TheLost>().Slow(glueLostSlowness);

                if (player.GetPlayerCharacter() == 3)
                {
                    if (damageTickTime > 0)
                    {
                        damageTickTime -= Time.deltaTime;
                    }
                    else
                    {
                        damageTickTime = tempTimer;
                        collision.GetComponent<TheLost>().DamageEnemy(glueDamage);
                    }
                }
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && collision.gameObject != player.gameObject)
        {
            collision.GetComponent<PlayerBody>().ExitGlue();
        }else if(collision.gameObject.tag == "Lost")
        {
            collision.GetComponent<TheLost>().ExitGlue();
        }
    }

    private IEnumerator Dissapear()
    {
        bullet.enabled = false;
        glueZone.enabled = true;
        yield return new WaitForSeconds(glueTime);
        gameObject.SetActive(false);
    }
}
