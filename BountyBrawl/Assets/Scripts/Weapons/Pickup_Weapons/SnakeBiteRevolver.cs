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
    [Tooltip("The sprite when player is holding the weapon")]
    [SerializeField] Sprite holding;
    private Sprite start; //The starting sprite before pickup
    private bool canFire = true; //Make sure the player can attack

    private bool canUse = true; // If the weapon can currently be used

    private PlayerBody player;
    private bool thrown = false; // If the player has thrown the weapon

    private BoxCollider2D weaponBody;

    private Throw nowThrow;

    private BoxCollider2D box;

    private void Awake()
    {
        weaponBody = GetComponent<BoxCollider2D>();
        box = GetComponent<BoxCollider2D>();
        nowThrow = GetComponent<Throw>();
        start = gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.getFire1() != 0 && ammo > 0)
            {
                if (player.getFire1() != 0 && ammo > 0 && !thrown)
                {
                    Shoot1();
                } //shoot gun if there is ammo and if player is holding the tringger
            }
            if (player.getThrow() != 0)
            {
                StartCoroutine(Throw());
            } //throw gun if player presses the circle button
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WeaponHolder" && canUse)
        {
            player = collision.transform.parent.GetComponent<PlayerBody>();
            //If player is not using a weapon
            if (!player.UsingWeapon())
            {
                player.ChangeWeapon(true);
                gameObject.GetComponent<SpriteRenderer>().sprite = holding;
                canUse = false;

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
        gameObject.GetComponent<SpriteRenderer>().sprite = start; //Reset the sprite
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