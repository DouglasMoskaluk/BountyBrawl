using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun_Bullet : MonoBehaviour
{

    [Tooltip("The amoung of time before the player can be damaged again")]
    [SerializeField] private float damageTickTime = 1f;

    [SerializeField] private float growSpeed = 100f; //Grow speed of beam

    private SpriteRenderer spriteRenderer;

    private BoxCollider2D boxCollider;

    private PlayerBody player; //The player that shot the weapon

    private Railgun gun;

    //Weaponry
    [SerializeField] private float baseDamage = 10f; //The base damage of the beam
    [SerializeField] private float fullLength = 400f; //The full length the beam can grow to

    private float tempTimer;
    private float speed;
    private float originalYSize;

    private bool deactivate;

    private List<GameObject> enemies;

    private void OnEnable()
    {
        deactivate = false;
        spriteRenderer.size = new Vector2(0f, originalYSize);
        speed = 0f;
        enemies.Clear();
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        gun = GetComponentInParent<Railgun>();
        tempTimer = damageTickTime;
        originalYSize = spriteRenderer.size.y;
        enemies = new List<GameObject>();
    }

    // Update is called once per frame
    private void Update()
    {
        boxCollider.size = spriteRenderer.size;
        boxCollider.offset = new Vector2((spriteRenderer.size.x / 2), 0);

        if(enemies.Count != 0)
        {
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
                        g.GetComponent<PlayerBody>().damagePlayer(baseDamage, player);
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

    void FixedUpdate()
    {
        if (spriteRenderer.size.x < fullLength)
        {
            speed += growSpeed * Time.deltaTime;
            spriteRenderer.size = new Vector2(speed, spriteRenderer.size.y);
        }

        if (deactivate)
        {
            enemies.Clear();
            gun.SetCanFire(false);
            gun.SetUsed(false);
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
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        enemies.Remove(collision.gameObject);
    }

    public void Deactivation(bool act) { deactivate = act; }
}
