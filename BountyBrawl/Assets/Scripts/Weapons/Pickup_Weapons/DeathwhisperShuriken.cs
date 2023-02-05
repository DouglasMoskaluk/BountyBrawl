using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DeathwhisperShuriken : MonoBehaviour
{
    [Tooltip("Distance the weapon floats from the player")]
    [SerializeField] private Transform bulletSpawn;
    [Tooltip("The time required before the next bullet is fired")]
    [SerializeField] private float bulletTime = 0.2f;
    [Tooltip("The amount of ammo the weapon has")]
    [SerializeField] private float ammo = 20f;
    [Tooltip("The sprites when player is holding the weapon for each individual player")]
    [SerializeField] Sprite[] holding = new Sprite[4];

    private List<GameObject> thrownShurikens; //Holds all the shurikens that have been fired

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

    private void Awake()
    {
        weaponBody = GetComponent<BoxCollider2D>();
        box = GetComponent<BoxCollider2D>();
        nowThrow = GetComponent<Throw>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        sprite = spriteRenderer.sprite;
    }
    private void OnEnable()
    {
        spriteRenderer.sortingOrder = 7;
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.getFire1() != 0 && ammo > 0 && !thrown)
            {
                Shoot1();
                //throw shuriken if there is ammo and if player is holding the trigger
            }
            if (player.getFire2() != 0 && thrownShurikens.Count != 0 && !thrown)
            {
                Shoot2();
                //shoot gun if there is ammo and if player is holding the tringger
            }
            if (player.getThrow() != 0)
            {
                StartCoroutine(Throw());
            } //throw weapon if player presses the circle button
        }
    }
    private void Shoot1()
    {
        if (canFire)
        {
            ammo--;
            canFire = false;
            Vector2 traj = bulletSpawn.position - transform.position; //Get the trajectory of the bullet
            traj.Normalize();
            GameObject bulletGO = ObjectPooler.Instance.SpawnFromPool("SnakeBiteRevolver_Bullet", bulletSpawn.position, transform.rotation);
            SnakeBiteRevolver_Bullet bull = bulletGO.GetComponent<SnakeBiteRevolver_Bullet>();
            bull.Fire(traj, player); //Pass the trajectory to the Fire method in the bullet script component
            bulletGO.SetActive(true);
            StartCoroutine(Cooldown(bulletTime));
        }
    }

    private void Shoot2()
    {
        //Recall the shurikens
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WeaponHolder" && canUse)
        {
            PlayerBody checker = collision.transform.parent.GetComponent<PlayerBody>();
            //If player is not using a weapon
            if (!checker.UsingWeapon())
            {
                spriteRenderer.sortingOrder = 10;
                player = collision.transform.parent.GetComponent<PlayerBody>();
                player.ChangeWeapon(true);
                canUse = false;

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
            spriteRenderer.sortingOrder = 7;
            gameObject.GetComponent<SpriteRenderer>().sprite = sprite; //Reset the sprite
            player.ChangeWeapon(false); //Set player back to default weapon
            transform.parent = null; //Unparent the weapon

            thrown = true;
            box.isTrigger = false;
            Vector2 traj = player.getLastFacing(); //Get the trajectory of the bullet

            yield return nowThrow.Cooldown(traj, player.gameObject); //Wait till the throwing motion is over
            thrown = false;
            box.isTrigger = true;
            player = null; //sets the player to null to wait for next player
            canUse = true; //Weapon is back to being pickupable
            weaponBody.isTrigger = true;
        } //Called when the weapon is being thrown
    }
}