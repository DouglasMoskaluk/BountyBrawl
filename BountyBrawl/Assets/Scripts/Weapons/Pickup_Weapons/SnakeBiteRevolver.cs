using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SnakeBiteRevolver : MonoBehaviour
{
    [Tooltip("Distance the weapon floats from the player")]
    [SerializeField] private Transform bulletSpawn;
    [Tooltip("The time required before the next bullet is fired")]
    [SerializeField] private float bulletTime = 0.2f;
    [Tooltip("The amount of ammo the weapon has")]
    [SerializeField] private float ammo = 20f;
    [Tooltip("The sprites when player is holding the weapon for each individual player")]
    [SerializeField] Sprite[] holding = new Sprite[4];
    [SerializeField] private float lifeTime = 20f;

    private Sprite sprite; //The starting sprite before pickup
    private SpriteRenderer spriteRenderer;
    private bool canFire = true; //Make sure the player can attack

    private bool canUse = true; // If the weapon can currently be used

    private PlayerBody player;
    private bool thrown = false; // If the player has thrown the weapon

    private BoxCollider2D weaponBody;

    private Throw nowThrow;

    private BoxCollider2D box;

    private bool canThrow;
    private float tempLifeTime;
    private float maxAmmo;

    private Animator animator;

    private void Awake()
    {
        weaponBody = GetComponent<BoxCollider2D>();
        box = GetComponent<BoxCollider2D>();
        nowThrow = GetComponent<Throw>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        sprite = spriteRenderer.sprite;
        spriteRenderer.sortingLayerName = "Midground";
        tempLifeTime = lifeTime;
        maxAmmo = ammo;
    }

    private void OnEnable()
    {
        animator.applyRootMotion = false;
        animator.SetTrigger("Idle");
        animator.SetBool("Fire1", false);
        animator.keepAnimatorControllerStateOnDisable = true;
        lifeTime = tempLifeTime;
        ammo = maxAmmo;
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.getFire1() != 0 && ammo > 0 && !thrown)
            {
                Shoot1();//shoot gun if there is ammo and if player is holding the tringger
            }
            if (player.getThrow() != 0 || ammo <= 0 && player.getFire1() != 0)
            {
                animator.applyRootMotion = true;
                animator.SetTrigger("Idle");
                animator.SetBool("Fire1", false);
                animator.SetBool("Reload", false);
                StartCoroutine(Throw());
            } //throw gun if player presses the circle button
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
        if (canFire)
        {
            animator.applyRootMotion = false;
            ammo--;
            canFire = false;
            Vector2 traj = bulletSpawn.position - transform.position; //Get the trajectory of the bullet
            traj.Normalize();
            GameObject bulletGO = ObjectPooler.Instance.SpawnFromPool("SnakeBiteRevolver_Bullet", bulletSpawn.position, transform.rotation);
            SnakeBiteRevolver_Bullet bull = bulletGO.GetComponent<SnakeBiteRevolver_Bullet>();
            bull.Fire(traj, player); //Pass the trajectory to the Fire method in the bullet script component
            bulletGO.SetActive(true);
            animator.SetBool("Fire1", true);
            StartCoroutine(Cooldown(bulletTime));
        }
    }

    public void Idle()
    {
        animator.applyRootMotion = true;
        animator.SetBool("Fire1", false);
        animator.SetTrigger("Idle");
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WeaponHolder" && canUse)
        {
            PlayerBody checker = collision.transform.parent.GetComponent<PlayerBody>();
            //If player is not using a weapon
            if (!checker.UsingWeapon())
            {
                spriteRenderer.sortingLayerName = "Foreground";
                player = collision.transform.parent.GetComponent<PlayerBody>();
                player.ChangeWeapon(true);
                canUse = false;
                player.setWeaponIndex(1);
                lifeTime = tempLifeTime;

                if (player.GetPlayerCharacter() == 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[0];
                }else if(player.GetPlayerCharacter() == 1)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[1];
                }
                else if (player.GetPlayerCharacter() == 2)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[2];
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[3];
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

                canThrow = true;
            }
        }
    }
    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canFire = true;
    }
    IEnumerator Throw()
    {
        if (canThrow)
        {
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

            if (ammo <= 0)
            {
                gameObject.SetActive(false);
            }

        } //Called when the weapon is being thrown
    }
}