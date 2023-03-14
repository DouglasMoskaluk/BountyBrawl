using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow_Bullet : MonoBehaviour
{

    [Tooltip("The amoung of time before the player can be damaged again")]
    [SerializeField] private float damageTickTime = 1f;

    [SerializeField] private float growSpeed = 100f; //Grow speed of beam

    private SpriteRenderer spriteRenderer;

    private BoxCollider2D boxCollider;

    private PlayerBody player; //The player that shot the weapon

    private Crossbow gun;

    //Weaponry
    [SerializeField] private float baseDamage = 10f; //The base damage of the beam
    [SerializeField] private float fullDamage = 30f; //The full damage of the beam
    [SerializeField] private float damageIncrease = 4f; //The damage increase 
    [SerializeField] private float damageIncreaseTime = 0.5f; //The time until the damage increases
    [SerializeField] private float currDamage;
    [SerializeField] private float fullLength = 50f; //The full length the beam can grow to

    private float tempTimer;
    private float tempDamTimer;
    private float speed;
    private float originalYSize;
    private float beamLength;
    private bool hitObstacle;

    private bool deactivate;
    private Vector2 endPosition;

    private List<GameObject> enemies;

    private void OnEnable()
    {
        deactivate = false;
        spriteRenderer.size = new Vector2(0f, originalYSize);
        speed = 0f;
        enemies.Clear();
        currDamage = baseDamage;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        gun = GetComponentInParent<Crossbow>();
        tempTimer = damageTickTime;
        originalYSize = spriteRenderer.size.y;
        enemies = new List<GameObject>();
        tempDamTimer = damageIncreaseTime;
    }

    // Update is called once per frame
    private void Update()
    {

        boxCollider.size = spriteRenderer.size;
        boxCollider.offset = new Vector2((spriteRenderer.size.x / 2), 0);

        if (enemies.Count != 0)
        {
            //Increase damage
            if (currDamage <= fullDamage)
            {
                if (damageIncreaseTime < 0)
                {
                    currDamage += damageIncrease;
                    damageIncreaseTime = tempDamTimer;
                }
                else
                {
                    damageIncreaseTime -= Time.deltaTime;
                }
            }

            if (damageTickTime > 0)
            {
                damageTickTime -= Time.deltaTime;
            }
            else
            {
                foreach (GameObject g in enemies.ToArray())
                {

                    if (g.tag == "Player" && g != player.gameObject)
                    {
                        g.GetComponent<PlayerBody>().damagePlayer(currDamage, player);
                    }
                    else if (g.tag == "Lost")
                    {
                        g.GetComponent<TheLost>().DamageEnemy(currDamage, player);
                    }
                    else if (g.tag == "Eater")
                    {
                        g.GetComponent<TheEater>().DamageEnemy(currDamage, player);
                    }
                }

                damageTickTime = tempTimer;
            }
        }
        else
        {
            currDamage = baseDamage;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, fullLength / 2);
        Debug.DrawRay(transform.position, transform.right * hit.distance, Color.red);

        if (hit.transform != null && hit.transform.tag == "Wall") //If the raycast has hit something and its tag is wall
        {

            if (hit.distance <= fullLength)
            {
                beamLength = hit.distance * 2;
            }
        }
        else
        {
            beamLength = fullLength;
        }
    }

    void FixedUpdate()
    {

        if (speed < beamLength)
        {
            speed += growSpeed * Time.deltaTime;
            spriteRenderer.size = new Vector2(speed, spriteRenderer.size.y);
        }
        else
        {
            spriteRenderer.size = new Vector2(beamLength, spriteRenderer.size.y);
        }

        if (deactivate)
        {
            gun.Reload();
            enemies.Clear();
            gun.SetCanFire(true);
            spriteRenderer.size = new Vector2(spriteRenderer.size.x, Mathf.Lerp(spriteRenderer.size.y, 0f, 0.5f));

            if(spriteRenderer.size.y < 0.1f)
            {
                damageTickTime = tempTimer;
                gun.SetCanFire(true);
                gameObject.SetActive(false);
            }
        }

    }

    public void Fire(PlayerBody play)
    {
        player = play; //Player dealing the damage
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemies.Add(collision.gameObject);

        if (collision.gameObject != player.gameObject && collision.transform.tag == "Player")
        {
            PlayerBody enemy = collision.GetComponent<PlayerBody>();

            //Damage player by base
            enemy.damagePlayer(baseDamage, player);

        }
        else if (collision.transform.tag == "Lost")
        {

            TheLost enemy = collision.GetComponent<TheLost>();

            //Damage lost by base
            enemy.DamageEnemy(baseDamage, player);

        }
        else if (collision.transform.tag == "Eater")
        {
            TheEater enemy = collision.GetComponent<TheEater>();

            //Damage eater by base
            enemy.DamageEnemy(baseDamage, player);
        }
        else if (collision.tag == "Box")
        {
            WeaponBox box = collision.GetComponent<WeaponBox>();
            player.IncreaseMoney(box.GetMoney());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemies.Remove(collision.gameObject);
    }

    public void Deactivation(bool act) { deactivate = act; }
}
