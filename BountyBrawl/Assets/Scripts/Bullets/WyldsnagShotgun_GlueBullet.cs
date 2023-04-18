using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyldsnagShotgun_GlueBullet : MonoBehaviour
{

    [Tooltip("The amoung of time before the player can be damaged again if player is emerald")]
    [SerializeField] private float damageTickTime = 1.5f;

    [SerializeField] private float speed = 50f; //Speed of bullet
    [SerializeField] private float bulletLifeTime = 1f; //Life until it dies

    [SerializeField] private Sprite glueShotSP; //The bullet sprite
    [SerializeField] private Sprite acidShotSP; //The bullet sprite
    [SerializeField] private Sprite glueSP; //The glue sprite
    [SerializeField] private Sprite acidSP; //The acid sprite for Emerald
    [SerializeField] private Gradient acidBulletTrail;
    [SerializeField] private Gradient glueBulletTrail;

    private SpriteRenderer glueRD;

    private Rigidbody2D bulletGO;

    private Vector3 direction;

    private float tempBulletLifeTime;
    private float tempGlueLifeTime;

    private Vector3 spawnPos;

    private PlayerBody player; //The player that shot the weapon

    private bool canGlue; 

    //Weaponry
    [SerializeField] private float baseDamage = 10f; //The base damage of the weapon without poison
    [SerializeField] private float stunLength = 1f; //The length of stun for players hit
    [SerializeField] public float glueTime = 5f; //The amount of time glue will stay in the level
    [SerializeField] private float glueDamage = 3f; //Amount of damage glue gives if player is emerald
    [SerializeField] private float gluePlayerSlowness = 5f; //How slow the glue will make players
    [SerializeField] private Vector3 glueSize = new Vector3(2f, 2f, 1f);
    [SerializeField] private float glueGrowSpeed = 3f;
    [SerializeField] private float glueLostSlowness = 300f;
    [SerializeField] private float glueEaterSlowness = 400f;
    private List<GameObject> enemies;

    [HideInInspector] public bool glue;
    private BoxCollider2D bullet; //The bullets hitbox
    private CircleCollider2D glueZone; //The glue hitbox
    private TrailRenderer trailRenderer;

    private float tempTimer;

    private void Awake()
    {
        glueRD = GetComponent<SpriteRenderer>();
        tempBulletLifeTime = bulletLifeTime;
        tempGlueLifeTime = glueTime;
        bullet = GetComponent<BoxCollider2D>();
        glueZone = GetComponent<CircleCollider2D>();
        tempTimer = damageTickTime;
        trailRenderer = GetComponent<TrailRenderer>();
        enemies = new List<GameObject>();
    }

    private void OnEnable()
    {
        glueRD.enabled = true;
        bulletLifeTime = tempBulletLifeTime;
        glueTime = tempGlueLifeTime;
        glue = false;
        bullet.enabled = true;
        glueZone.enabled = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
        canGlue = true;
        enemies.Clear();
    }

    private void Start()
    {
        bulletGO = gameObject.GetComponent<Rigidbody2D>();
        spawnPos = transform.position;

        if (player.getCharacter() == 3)
        {
            glueRD.sprite = acidShotSP;
            trailRenderer.colorGradient = acidBulletTrail;
        }
        else
        {
            glueRD.sprite = glueShotSP;
            trailRenderer.colorGradient = glueBulletTrail;
        }
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

        //If the bullet is a bullet and not glue
        if(bulletLifeTime < 0 && !glue)
        {
            //If bullet is not off map
            if (!canGlue) 
            {
                gameObject.SetActive(false);
            }

            glue = true;
            transform.rotation = Quaternion.identity;
            transform.localScale = new Vector3(1f, 1.02f, 1f);

            if (player.GetPlayerCharacter() == 3)
            {
                glueRD.sprite = acidSP;
                bulletLifeTime = tempBulletLifeTime;
            }
            else
            {
                bulletLifeTime = tempBulletLifeTime;
                glueRD.sprite = glueSP;
            }
        }
        else
        {
            bulletLifeTime -= Time.deltaTime;
        }

        //If glue is true
        if(glue && glueTime <= 0)
        {

            gameObject.SetActive(false);
            glueTime = tempGlueLifeTime;
        }
        else
        {
            glueTime -= Time.deltaTime;

            if(player.getCharacter() == 3)
            {
                //Damages all enemies in the acid
                if (enemies.Count != 0)
                {
                    if (damageTickTime > 0)
                    {
                        damageTickTime -= Time.deltaTime;
                    }
                    else
                    {
                        foreach (GameObject g in enemies)
                        {

                            if (g.tag == "Player" && g != player.gameObject)
                            {
                                g.GetComponent<PlayerBody>().damagePlayer(glueDamage, player);
                            }
                            else if (g.tag == "Lost")
                            {
                                g.GetComponent<TheLost>().DamageEnemy(baseDamage, player);
                            }
                            else if (g.tag == "Eater")
                            {
                                g.GetComponent<TheEater>().DamageEnemy(baseDamage, player);
                            }
                        }

                        damageTickTime = tempTimer;
                    }
                }
            } 
        }

        //Change size of poison area over time
        if (transform.localScale.x < glueSize.x && glue)
        {
            Vector3 temp = transform.localScale;
            temp.x += Time.deltaTime * glueGrowSpeed;
            temp.z += Time.deltaTime * glueGrowSpeed;

            transform.localScale = temp;

        }
    }

    public void Fire(Vector3 dir, PlayerBody play)
    {
        direction = dir; //assigns given direction to direction variable.
        player = play; //If the player is profficient

        if(player.getCharacter() == 3)
        {
            glueRD.sprite = acidShotSP;
        }
        else
        {
            glueRD.sprite = glueShotSP;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!glue)
        {
            bullet.enabled = false;
            glueZone.enabled = true;

            //Hit a wall
            if (collision.gameObject.tag == "Wall")
            {
                GetComponent<TrailRenderer>().enabled = false;
                glue = true;
                StartCoroutine(Dissapear());
                transform.rotation = Quaternion.identity;
                transform.localScale = new Vector3(1f, 1.02f, 1f);

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

            //Hit a player
            else if (collision.gameObject != player.gameObject && collision.transform.tag == "Player")
            {
                PlayerBody enemy = collision.GetComponent<PlayerBody>();
                transform.localScale = new Vector3(1f, 1.02f, 1f);

                //Damage player by base
                enemy.damagePlayer(baseDamage, player);
                enemy.StartCoroutine(enemy.Stun(stunLength));
                StartCoroutine(Dissapear());
                transform.rotation = Quaternion.identity;
                enemy.Slow(gluePlayerSlowness);

                glue = true;

                //If player is proficient
                if (player.GetPlayerCharacter() == 3)
                {
                    enemies.Add(collision.gameObject);
                    glueRD.sprite = acidSP;
                }
                else
                {
                    glueRD.sprite = glueSP;
                }

                //Hit a Lost
            }else if(collision.transform.tag == "Lost")
            {

                TheLost enemy = collision.GetComponent<TheLost>();
                transform.rotation = Quaternion.identity;
                transform.localScale = new Vector3(1f, 1.02f, 1f);
                enemy.Slow(glueLostSlowness);

                //Damage player by base
                enemy.DamageEnemy(baseDamage, player);
                enemy.StartCoroutine(enemy.Stun(stunLength));
                StartCoroutine(Dissapear());

                glue = true;

                //If player is proficient
                if (player.GetPlayerCharacter() == 3)
                {
                    enemies.Add(collision.gameObject);
                    glueRD.sprite = acidSP;
                }
                else
                {
                    glueRD.sprite = glueSP;
                }

                //Hit an Eater
            }else if (collision.transform.tag == "Eater")
            {
                TheEater enemy = collision.GetComponent<TheEater>();
                transform.rotation = Quaternion.identity;
                transform.localScale = new Vector3(1f,1.02f,1f);
                enemy.Slow(glueEaterSlowness);

                //Damage player by base
                enemy.DamageEnemy(baseDamage, player);
                enemy.StartCoroutine(enemy.Stun(stunLength));
                StartCoroutine(Dissapear());

                glue = true;

                //If player is proficient
                if (player.GetPlayerCharacter() == 3)
                {
                    enemies.Add(collision.gameObject);
                    glueRD.sprite = acidSP;
                }
                else
                {
                    glueRD.sprite = glueSP;
                }
            }
            else if (collision.tag == "Box")
            {
                WeaponBox box = collision.GetComponent<WeaponBox>();
                player.IncreaseMoney(box.GetMoney());
            }

            //Hit outside barrier
            else if (collision.gameObject.tag == "Barrier")
            {
                canGlue = false;
            }
        }
        else
        {
            if (player.getCharacter() == 3)
            {
                if (collision.gameObject != player.gameObject && collision.transform.tag == "Player")
                {
                    collision.GetComponent<PlayerBody>().Slow(gluePlayerSlowness);
                    if (!enemies.Contains(collision.gameObject))
                    {
                        enemies.Add(collision.gameObject);
                    }
                }else if(collision.gameObject.tag == "Lost")
                {
                    collision.GetComponent<TheLost>().Slow(glueLostSlowness);
                    if (!enemies.Contains(collision.gameObject))
                    {
                        enemies.Add(collision.gameObject);
                    }
                }
                else if (collision.gameObject.tag == "Eater")
                {
                    collision.GetComponent<TheEater>().Slow(glueEaterSlowness);
                    if (!enemies.Contains(collision.gameObject))
                    {
                        enemies.Add(collision.gameObject);
                    }
                }
            }
            else
            {
                if (collision.gameObject != player.gameObject && collision.transform.tag == "Player")
                {
                    collision.GetComponent<PlayerBody>().Slow(gluePlayerSlowness);
                }
                else if (collision.gameObject.tag == "Lost")
                {
                    collision.GetComponent<TheLost>().Slow(glueLostSlowness);
                }
                else if (collision.gameObject.tag == "Eater")
                {
                    collision.GetComponent<TheEater>().Slow(glueEaterSlowness);
                }
            }

            if (collision.tag == "Bullet")
            {
                if (collision.GetComponent<WyldsnagShotgun_GlueBullet>() != null)
                {
                    WyldsnagShotgun_GlueBullet hit = collision.GetComponent<WyldsnagShotgun_GlueBullet>();
                    if (hit.glue && this.glueTime > hit.glueTime)
                    {
                        Destroy(collision.gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemies.Remove(collision.gameObject);

        if(collision.gameObject.tag == "Player" && collision.gameObject != player.gameObject)
        {
            collision.GetComponent<PlayerBody>().ExitGlue(gluePlayerSlowness);
        }else if(collision.gameObject.tag == "Lost")
        {
            collision.GetComponent<TheLost>().ExitGlue(glueLostSlowness);
        }else if (collision.gameObject.tag == "Eater")
        {
            collision.GetComponent<TheEater>().ExitGlue(glueEaterSlowness);
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
