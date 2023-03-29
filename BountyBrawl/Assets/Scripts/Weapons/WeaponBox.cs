using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    [SerializeField] private float commonDropChance = 70; //Common weapon drop chance once rares can be spawned
    [SerializeField] private int numTillRare = 3; //Number of weapon boxes needing to be spawned until rare weapons spawn
    [SerializeField] private float moneyGiven = 5;

    private int numSpawn; //number of times weapon boxes have spawned
    public int weapon; //The weapon to be spawned

    private WeaponBox[] weaponBoxes;

    private int hit;

    private void Awake()
    {
        numSpawn = 0;
        weaponBoxes = FindObjectsOfType<WeaponBox>();
        hit = 1;
    }

    private void OnEnable()
    {
        hit = 1;

        //Increase number of weapon spawns for each weapon box
        foreach (WeaponBox p in weaponBoxes)
        {
            p.IncreaseWeaponBox();
        }

        if(numSpawn < numTillRare) //Spawn common weapon
        {
            weapon = (int) Random.Range(0f, 3f);
        }
        else //Can spawn rare weapons
        {
            int chance = (int) Random.Range(1f, 101f);

            if(chance <= commonDropChance)
            {
                weapon = (int)Random.Range(0f, 3f);
            }else
            {
                weapon = (int)Random.Range(3f, 7f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet" || collision.tag == "Special Bullet")
        {
                hit--;

            if (hit == 0)
            {
                if (weapon == 0) //Spawn common weapon 1
                {
                    ObjectPooler.Instance.SpawnFromPool("SnakeBiteRevolver", transform.position, transform.rotation);
                    gameObject.SetActive(false);
                }
                else if (weapon == 1) //Spawn common weapon 2
                {
                    ObjectPooler.Instance.SpawnFromPool("WyldsnagShotgun", transform.position, transform.rotation);
                    gameObject.SetActive(false);
                }
                else if (weapon == 2)//Spawn common weapon 3
                {
                    ObjectPooler.Instance.SpawnFromPool("Ch-ChingRifle", transform.position, transform.rotation);
                    gameObject.SetActive(false);
                }
                else if (weapon == 3) //Spawn rare weapon 1
                {
                    ObjectPooler.Instance.SpawnFromPool("DeathWhisperShuriken", transform.position, transform.rotation);
                    gameObject.SetActive(false);
                }
                else if (weapon == 4) //Spawn rare weapon 2
                {
                    ObjectPooler.Instance.SpawnFromPool("Crossbow", transform.position, transform.rotation);
                    gameObject.SetActive(false);
                }
                else //Spawn rare weapon 3
                {
                    ObjectPooler.Instance.SpawnFromPool("Railgun", transform.position, transform.rotation);
                    gameObject.SetActive(false);
                }

                if (collision.tag != "Special Bullet")
                {
                    collision.gameObject.SetActive(false);
                }
            }
        }
    }

    public void IncreaseWeaponBox() { numSpawn++; }

    public float GetMoney() {
        if (hit == 0)
        { 
            return moneyGiven;
        }
        else
        {
            return 0;
        }

    }
}
