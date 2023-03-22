using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Emerald_Hammer : MonoBehaviour
{

    [Tooltip("The time required before the slam can be used again")]
    [SerializeField] private float slamCooldown = 5f;

    [SerializeField] private float damage = 19;

    private CircleCollider2D attack;

    [SerializeField] private PlayerBody player; //This is the player

    [SerializeField] private Transform weaponHolder; //The thing that holds the weapon

    private bool canFire = true; //Make sure the player can attack

    private bool slamming = false; //Whether the player is slamming

    private float currAngle;
    private float currTime;

    [SerializeField] private AudioSource banging;
    [SerializeField] private AudioClip[] soundClips;

    private float tempCooldown;
    private void Awake()
    {
        attack = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        canFire = false;
        StartCoroutine(Cooldown(0.5f));
        player.maxAmmo = slamCooldown;
        tempCooldown = slamCooldown;
        player.currAmmo = slamCooldown;
    }

    private void OnDisable()
    {
        attack.enabled = false;
        slamming = false;
        currTime = 0f;
        player.EmeraldHammer(false);
        StopAllCoroutines();
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.getFire1() != 0 && canFire)
            {
                player.StartAttack();
                Shoot1();
            }


            if (player.maxAmmo > slamCooldown)
            {
                tempCooldown = slamCooldown;
                player.maxAmmo = slamCooldown;
            }

            if (!canFire)
            {
                if(tempCooldown < slamCooldown)
                {
                    tempCooldown += Time.deltaTime;
                    player.currAmmo = tempCooldown;
                }
            }

        }
        if (slamming && attack.isActiveAndEnabled)
        {
            slamming = false;
            PlayRandSound();
        }

    }

    private void Shoot1()
    {
        tempCooldown = 0;
        canFire = false;
        slamming = true;
        player.EmeraldHammer(true);
        StartCoroutine(Cooldown(slamCooldown));
    }

    IEnumerator Cooldown(float time)
    { 
        yield return new WaitForSeconds(time);
        canFire = true;
    } //When the player can use the dash again

    public void PlayRandSound()
    {
        int rand = (int)Random.Range(0f, soundClips.Length);
        banging.clip = soundClips[rand];
        banging.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != player && other.tag == "Player")
        {
            other.GetComponent<PlayerBody>().damagePlayer(damage, player);
        }
        if (other.tag == "Lost")
        {
            other.GetComponent<TheLost>().DamageEnemy(damage, player);
        }
        if (other.tag == "Eater")
        {
            other.GetComponent<TheEater>().DamageEnemy(damage, player);
        }
    }

}
