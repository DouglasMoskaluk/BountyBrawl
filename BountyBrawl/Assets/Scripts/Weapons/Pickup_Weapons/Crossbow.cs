using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Crossbow : MonoBehaviour
{
    [Tooltip("The lightning beam being fired")]
    [SerializeField] private Crossbow_Bullet beam;
    [Tooltip("The use timer")]
    [SerializeField] private float timer = 2f;
    [Tooltip("The sprites when player is holding the weapon for each individual player")]
    [SerializeField] Sprite[] holding = new Sprite[4];
    [SerializeField] private float lifeTime = 20f;
    [SerializeField] private GameObject crossbowHead;

    private Sprite sprite; //The starting sprite before pickup
    private SpriteRenderer spriteRenderer;
    private bool firing = true; //Make sure the player can attack

    private bool canUse = true; // If the weapon can currently be used

    private PlayerBody player;
    private bool thrown = false; // If the player has thrown the weapon

    private BoxCollider2D weaponBody;

    private Throw nowThrow;

    private BoxCollider2D box;

    private bool canThrow;
    private bool canShoot;
    private float tempTimer;
    private float tempLifeTime;

    private Animator animator;

    [SerializeField] private AudioSource normFire;
    [SerializeField] private AudioSource looping;
    [SerializeField] private AudioSource throwing;

    private void Awake()
    {
        weaponBody = GetComponent<BoxCollider2D>();
        box = GetComponent<BoxCollider2D>();
        nowThrow = GetComponent<Throw>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        sprite = spriteRenderer.sprite;
        canShoot = true;
        spriteRenderer.sortingLayerName = "Midground";
        firing = false;
        tempTimer = timer;
        tempLifeTime = lifeTime;
    }

    private void OnEnable()
    {
        Idle();
        animator.enabled = false;
        timer = tempTimer;
        lifeTime = tempLifeTime;
        crossbowHead.SetActive(false);
    }

    private void Update()
    {
        if (player != null)
        {
            player.maxAmmo = tempTimer;
            player.currAmmo = timer;

            if (player.getFire1() != 0 && timer > 0 && !thrown && canShoot)
            {
                if (!firing) 
                { 
                    normFire.Play();
                }

                //beam.Deactivation(false);
                crossbowHead.SetActive(true);
                firing = true;
                if (!beam.isActiveAndEnabled)
                {
                    animator.SetBool("Fire1", true);
                }
                animator.enabled = true;
                Shoot1();//shoot gun if there is ammo and if player is holding the trigger
            }
            else if (player.getFire1() == 0 || timer < 0 || thrown || !canShoot)
            {
                //beam.Deactivation(true);
                beam.gameObject.SetActive(false);

                if (firing == true)
                {
                    canShoot = true;
                    firing = false;
                }
            }
            if (player.getThrow() != 0 || timer <= 0 && player.getFire1() != 0)
            {
                Idle();
                animator.enabled = true;
                StartCoroutine(Throw());
            } //throw gun if player presses the circle button

            //Drops weapon if player dies
            if (player.getHealth() <= 0)
            {
                normFire.Stop();
                looping.Stop();
                player.ChangeWeapon(false); //Set player back to default weapon
                player = null; //sets the player to null to wait for next player
                //beam.Deactivation(true);
                firing = false;
                transform.parent = null; //Unparent the weapon
                thrown = false;
                box.isTrigger = true;
                canUse = true; //Weapon is back to being pickupable
                spriteRenderer.sprite = sprite; //Reset the sprite
                weaponBody.isTrigger = true;
                spriteRenderer.sortingLayerName = "Midground";

                Idle();
                animator.enabled = true;
                canShoot = true;
                crossbowHead.SetActive(false);

                beam.gameObject.SetActive(false);
            }
        }

        if (firing)
        {
            timer -= Time.deltaTime;
        }

        if (spriteRenderer.sortingLayerName == "Midground")
        {
            if (lifeTime < 0)
            {
                lifeTime = tempLifeTime;
                gameObject.SetActive(false);
            }
            else
            {
                lifeTime -= Time.deltaTime;
            }
        }
    }

    private void Shoot1()
    {
        beam.Fire(player); //Pass the player   
    }

    public void Fire2()
    {
        beam.gameObject.SetActive(true);
        animator.SetBool("Fire1", false);
        animator.SetBool("Fire2", true);
        normFire.Stop();
        looping.Play();
    }

    public void Idle()
    {
        crossbowHead.SetActive(false);
        animator.SetBool("Fire1", false);
        animator.SetBool("Fire2", false);
        animator.SetBool("Reload", false);
        animator.SetTrigger("Idle");
        animator.enabled = false;
        looping.Stop();

        if (!thrown)
        {
            crossbowHead.SetActive(true);
        }
    }

    public void Reload()
    {
        animator.SetBool("Fire1", false);
        animator.SetBool("Fire2", false);
        animator.SetBool("Reload", true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "WeaponHolder" && canUse && !thrown)
        {
            PlayerBody checker = collision.transform.parent.GetComponent<PlayerBody>();
            //If player is not using a weapon
            if (!checker.UsingWeapon())
            {
                crossbowHead.SetActive(true);
                spriteRenderer.sortingLayerName = "Foreground";
                animator.enabled = false;

                player = collision.transform.parent.GetComponent<PlayerBody>();
                player.ChangeWeapon(true);
                canUse = false;
                thrown = false;
                canShoot = true;
                player.setWeaponIndex(1);

                if (player.GetPlayerCharacter() == 0)
                {
                    spriteRenderer.sprite = holding[0];
                }else if(player.GetPlayerCharacter() == 1)
                {
                    spriteRenderer.sprite = holding[1];
                }
                else if (player.GetPlayerCharacter() == 2)
                {
                    spriteRenderer.sprite = holding[2];
                }
                else
                {
                    spriteRenderer.sprite = holding[3];
                }

                //Move weapon in desired position
                if (collision.transform.right.x >= 0)
                {
                    transform.position = collision.transform.position + collision.transform.right * 0.2f;
                }
                else
                {
                    transform.position = collision.transform.position + collision.transform.right * 0.2f;
                }
                transform.rotation = collision.transform.rotation;
                transform.parent = collision.GetComponentInChildren<CircleCollider2D>().transform;

                StartCoroutine(CanThrow());
            }
        }
    }

    IEnumerator CanThrow()
    {
        yield return new WaitForSeconds(0.2f);
        canThrow = true;
    }
    IEnumerator Throw()
    {
        if (canThrow)
        {
            crossbowHead.SetActive(false);
            normFire.Stop();
            looping.Stop();
            throwing.Play();
            canThrow = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite; //Reset the sprite
            player.ChangeWeapon(false); //Set player back to default weapon
            transform.parent = null; //Unparent the weapon

            thrown = true;
            box.isTrigger = false;
            Vector2 traj = player.getLastFacing(); //Get the trajectory of the bullet

            yield return nowThrow.Cooldown(traj, player.gameObject); //Wait till the throwing motion is over
            spriteRenderer.sortingLayerName = "Midground";
            thrown = false;
            box.isTrigger = true;
            player = null; //sets the player to null to wait for next player
            canUse = true; //Weapon is back to being pickupable
            weaponBody.isTrigger = true;

            if (timer <= 0)
            {
                gameObject.SetActive(false);
            }

        } //Called when the weapon is being thrown
    }

    public void SetCanFire(bool can) { canShoot = can; }
}