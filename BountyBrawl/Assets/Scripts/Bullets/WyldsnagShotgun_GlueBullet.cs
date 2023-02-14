using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WyldsnagShotgun_GlueBullet : MonoBehaviour
{

    [Tooltip("The amoung of time before the player can be damaged again if player is emerald")]
    [SerializeField] private float damageTickTime = 1.5f;

    [SerializeField] private float speed = 50f; //Speed of bullet
    [SerializeField] private float bulletLifeTime = 1f; //Life until it dies
    [SerializeField] private float glueLifeTime = 2f; //Life until it dies

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
    [SerializeField] private float glueTime = 5f; //The amount of time glue will stay in the level
    [SerializeField] private float glueDamage = 3f; //Amount of damage glue gives if player is emerald
    [SerializeField] private float gluePlayerSlowness = 9f; //How slow the glue will make players
    [SerializeField] private Vector3 glueSize = new Vector3(2f, 2f, 1f);
    [SerializeField] private float glueGrowSpeed = 3f;
    [SerializeField] private float glueLostSlowness = 500f;
    [SerializeField] private float glueEaterSlowness = 400f;

    private bool glue;
    private BoxCollider2D bullet; //The bullets hitbox
    private CircleCollider2D glueZone; //The glue hitbox
    private TrailRenderer trailRenderer;

    private float tempTimer;

    private void Awake()
    {
        glueRD = GetComponent<SpriteRenderer>();
        tempBulletLifeTime = bulletLifeTime;
        tempGlueLifeTime = glueLifeTime;
        bullet = GetComponent<BoxCollider2D>();
        glueZone = GetComponent<CircleCollider2D>();
        tempTimer = damageTickTime;
        trailRenderer = GetComponent<TrailRenderer>();
    }

    private void OnEnable()
    {
        glueRD.enabled = true;
        bulletLifeTime = tempBulletLifeTime;
        glueLifeTime = tempGlueLifeTime;
        glue = false;
        bullet.enabled = true;
        glueZone.enabled = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
        canGlue = true;
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

        if((bulletLifeTime -= Time.deltaTime) < 0 && !glue)
        {
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
            }
            else
            {
                glueRD.sprite = glueSP;
            }
        }

        if(glue && (glueLifeTime -= Time.deltaTime) < 0)
        {
            gameObject.SetActive(false);
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
            else if (collision.gameObject != player.gameObject && collision.transform.tag == "Player")
            {
                PlayerBody enemy = collision.GetComponent<PlayerBody>();
                transform.localScale = new Vector3(1f, 1.02f, 1f);

                //Damage player by base
                enemy.damagePlayer(baseDamage, player);
                enemy.StartCoroutine(enemy.Stun(stunLength));
                StartCoroutine(Dissapear());
                transform.rotation = Quaternion.identity;

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
                transform.rotation = Quaternion.identity;
                transform.localScale = new Vector3(1f, 1.02f, 1f);

                //Damage player by base
                enemy.DamageEnemy(baseDamage, player);
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
            }else if (collision.transform.tag == "Eater")
            {
                TheEater enemy = collision.GetComponent<TheEater>();
                transform.rotation = Quaternion.identity;
                transform.localScale = new Vector3(1f,1.02f,1f);

                //Damage player by base
                enemy.DamageEnemy(baseDamage, player);
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
            else if (collision.gameObject.tag == "Barrier")
            {
                canGlue = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (glue)
        {

            TheLost tempLost = null;
            TheEater tempEater = null;
            PlayerBody enemy = null;

            if (collision.GetComponent<TheLost>() != null)
            {
                tempLost = collision.GetComponent<TheLost>();
                tempLost.Slow(glueLostSlowness);
            }

            if (collision.GetComponent<TheEater>() != null)
            {
                tempEater = collision.GetComponent<TheEater>();
                tempEater.Slow(glueEaterSlowness);
            }

            if (collision.gameObject.tag == "Player" && collision.gameObject != player.gameObject)
            {

                enemy = collision.GetComponent<PlayerBody>();
                enemy.Slow(gluePlayerSlowness);
            }

            //Damage player if character who shot is emerald and if enemy is in acid 
            if (player.GetPlayerCharacter() == 3)
            {
                if (damageTickTime > 0)
                {
                    damageTickTime -= Time.deltaTime;
                }
                else
                {
                    damageTickTime = tempTimer;
                    if(tempLost != null) { tempLost.DamageEnemy(glueDamage, player); }
                    if (tempEater != null) { tempEater.DamageEnemy(glueDamage, player); }
                    if (enemy != null) { enemy.damagePlayer(glueDamage, player); }
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
        }else if (collision.gameObject.tag == "Eater")
        {
            collision.GetComponent<TheEater>().ExitGlue();
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
