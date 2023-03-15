using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ch_ChingRifle : MonoBehaviour
{
    [Tooltip("Spawnpoint of all the bullets")]
    [SerializeField] private Transform bulletSpawn;
    [Tooltip("The time required before the next bullet is fired")]
    [SerializeField] private float bulletTime = 0.2f;
    [Tooltip("The amount of ammo the weapon has")]
    [SerializeField] private float ammo = 20f;
    [Tooltip("The sprites when player is holding the weapon for each individual player")]
    [SerializeField] Sprite[] holding = new Sprite[4];
    [Tooltip("The number of bullets in cluster")]
    [SerializeField] private int bulletsInCluster = 5;
    [SerializeField] private float knockBackStrength = 30f;
    [SerializeField] private float lifeTime = 20f; //Time until it despawns

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
        lifeTime = tempLifeTime;
        Idle();
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.getFire1() != 0 && ammo > 0 && !thrown)
            {
                Shoot1(); //shoot gun if there is ammo and if player is holding the tringger
            }
            if (player.getFire2() != 0 && ammo >= 2 && !thrown)
            {
                Shoot2();
                //shoot gun if there is ammo and if player is holding the tringger
            }
            if (player.getThrow() != 0 || ammo <= 0 && player.getFire1() != 0)
            {
                Idle();
                StartCoroutine(Throw());
            } //throw gun if player presses the circle button

            //Drops weapon if player dies
            if (player.getHealth() <= 0)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = sprite; //Reset the sprite
                player.ChangeWeapon(false); //Set player back to default weapon
                transform.parent = null; //Unparent the weapon
                spriteRenderer.sortingLayerName = "Midground";
                thrown = false;
                box.isTrigger = true;
                player = null; //sets the player to null to wait for next player
                canUse = true; //Weapon is back to being pickupable
                weaponBody.isTrigger = true;
            }
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
            ammo--;
            canFire = false;
            animator.SetBool("Fire1", true);

            Vector2 traj = bulletSpawn.position - player.transform.position; //Get the trajectory of the bullet
            traj.Normalize();
            GameObject bulletGO = ObjectPooler.Instance.SpawnFromPool("Ch-ChingRifle_DefBullet", bulletSpawn.position, transform.rotation);
            Ch_ChingRifle_DefBullet bull = bulletGO.GetComponent<Ch_ChingRifle_DefBullet>();
            bull.Fire(traj, player); //Pass the trajectory to the Fire method in the bullet script component
            bulletGO.SetActive(true);
            StartCoroutine(Cooldown(bulletTime));
            player.StartCoroutine(player.Knocked(knockBackStrength, -traj.normalized));
        }
    }

    private void Shoot2()
    {
        if (canFire)
        {
            ammo -= 4;
            canFire = false;

            animator.SetBool("Fire2", true);

            for (int i = 0; i < bulletsInCluster; i++)
            {
                Vector2 traj = bulletSpawn.position - player.transform.position; //Get the trajectory of the bullet
                traj.Normalize();
                GameObject bulletGO = ObjectPooler.Instance.SpawnFromPool("Ch-ChingRifle_ClusBullet", bulletSpawn.position, transform.rotation);
                Ch_ChingRifle_ClusBullet bull = bulletGO.GetComponent<Ch_ChingRifle_ClusBullet>();
                bull.Fire(traj, player); //Pass the trajectory to the Fire method in the bullet script component
                bulletGO.SetActive(true);
                StartCoroutine(Cooldown(bulletTime * 5f));
            }
        }
    }

    public void Idle()
    {
        animator.SetBool("Fire1", false);
        animator.SetBool("Fire2", false);
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
                player.setWeaponIndex(2);
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

        }
    } //Called when the weapon is being thrown
}