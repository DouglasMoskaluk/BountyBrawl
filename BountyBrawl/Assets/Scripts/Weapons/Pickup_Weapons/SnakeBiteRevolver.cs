using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBiteRevolver : MonoBehaviour
{
    [Tooltip("Distance the weapon floats from the player")]
    [SerializeField] private Transform bulletSpawn;

    [Tooltip("The prefab of the bullet to spawn")]
    [SerializeField] private GameObject bullet;

    [Tooltip("The time required before the next bullet is fired")]
    [SerializeField] private float bulletTime = 0.2f;

    [Tooltip("The amount of ammo the weapon has")]
    [SerializeField] private float ammo = 20f;

    [Tooltip("The sprite when player is holding the weapon")]
    [SerializeField] Sprite holding;

    private bool canFire = true; //Make sure the player can;

    private PlayerBody player;

    private void Update()
    {
        if (player != null)
        {
            if (player.getFire1() != 0)
            {
                Shoot1();
            }
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
            GameObject bulletGO = GameObject.Instantiate(bullet.gameObject, bulletSpawn.position, transform.rotation);
            SnakeBiteRevolver_Bullet bull = bulletGO.GetComponent<SnakeBiteRevolver_Bullet>();
            bull.Fire(traj); //Pass the trajectory to the Fire method in the bullet script component
            bulletGO.SetActive(true);
            StartCoroutine(Cooldown(bulletTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "WeaponHolder")
        {

            player = collision.transform.parent.GetComponent<PlayerBody>();

            if (!player.weapon)
            {
                player.weapon = true;
                gameObject.GetComponent<SpriteRenderer>().sprite = holding;

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
}
