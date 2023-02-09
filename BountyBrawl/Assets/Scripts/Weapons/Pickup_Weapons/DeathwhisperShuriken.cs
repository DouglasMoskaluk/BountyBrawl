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
    [Tooltip("The amount of shurikens proficiency character can have on screen before return")]
    [SerializeField] private int profThrown = 5;
    [Tooltip("The amount of shurikens non-proficiency character can have on screen before return")]
    [SerializeField] private int nonProfThrown = 3;
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
    private bool returning;

    private int thrownLimit; //The limit of how many shurikens can be thrown before returning

    private void Awake()
    {
        weaponBody = GetComponent<BoxCollider2D>();
        box = GetComponent<BoxCollider2D>();
        nowThrow = GetComponent<Throw>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        thrownShurikens = new List<GameObject>();
        sprite = spriteRenderer.sprite;
        returning = false;
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

        if(thrownShurikens != null && thrownShurikens.Count != 0)
        {
            for(int i = 0; i < thrownShurikens.Count; i++)
            {
                if(thrownShurikens[i].activeSelf == false)
                {
                    thrownShurikens.RemoveAt(i);
                }
            }
        }

        if (returning)
        {
            if(thrownShurikens.Count == 0)
            {
                returning = false;
            }
        }
    }

    private void Shoot1()
    {

        if(canFire && thrownShurikens.Count >= thrownLimit)
        {
            returning = false;
            Shoot2();
        }

        if (canFire && !returning )
        {
            ammo--;
            canFire = false;
            Vector2 traj = bulletSpawn.position - transform.position; //Get the trajectory of the bullet
            traj.Normalize();
            GameObject bulletGO = ObjectPooler.Instance.SpawnFromPool("Deathwhisper_Shuriken", bulletSpawn.position, transform.rotation);
            Deathwhisper_ShurikenBullet bull = bulletGO.GetComponent<Deathwhisper_ShurikenBullet>();
            thrownShurikens.Add(bulletGO);
            bull.Fire(traj, player); //Pass the trajectory to the Fire method in the bullet script component
            bulletGO.SetActive(true);
            StartCoroutine(Cooldown(bulletTime));
        }
        

    }

    private void Shoot2()
    {
        returning = true; //Wait till all shurikens return before shooting again
        for(int i = 0; i < thrownShurikens.Count; i++)
        {
            Deathwhisper_ShurikenBullet returning = thrownShurikens[i].GetComponent<Deathwhisper_ShurikenBullet>();
            returning.ReturnToPlayer();
        }
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
                player.setWeaponIndex(3);

                if (player.GetPlayerCharacter() == 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[0];
                }else if(player.GetPlayerCharacter() == 1)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[0];
                }
                else if (player.GetPlayerCharacter() == 2)
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[0];
                }
                else
                {
                    gameObject.GetComponent<SpriteRenderer>().sprite = holding[0];
                }

                if(player.getCharacter() == 1)
                {
                    thrownLimit = profThrown;
                }
                else
                {
                    thrownLimit = nonProfThrown;
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
            foreach(GameObject p in thrownShurikens)
            {
                p.GetComponent<Deathwhisper_ShurikenBullet>().Thrown();
            }

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