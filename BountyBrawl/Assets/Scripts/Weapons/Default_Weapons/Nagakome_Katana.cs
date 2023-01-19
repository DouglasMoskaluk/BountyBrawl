using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Nagakome_Katana : MonoBehaviour
{

    [Tooltip("The time required before the slashes can be used again")]
    [SerializeField] private float slashCooldown = 3f;

    [Tooltip("Speed at which the slashes happen")]
    [SerializeField] private float slashSpeed = 1f;

    private BoxCollider2D attack;

    [SerializeField] private PlayerBody player; //This is the player

    [SerializeField] private float damage = 9;

    private bool canFire = true; //Make sure the player can attack


    private void Awake()
    {
        attack = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        canFire = true;
    }

    private void OnDisable()
    {
        attack.enabled = false;
        player.UsingDefault(false);
        StopAllCoroutines();
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.getFire1() != 0 && canFire)
            {
                Shoot1();
            }
        }
    }

    private void Shoot1()
    {
        canFire = false;
        player.UsingDefault(true);
        StartCoroutine(Slash(slashSpeed));
    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canFire = true;
    } //When the player can use the dash again

    IEnumerator Slash(float time)
    {
        attack.enabled = true;
        yield return new WaitForSeconds(0.05f);
        attack.enabled = false;
        yield return new WaitForSeconds(time);
        attack.enabled = true;
        yield return new WaitForSeconds(0.05f);
        attack.enabled = false;
        player.UsingDefault(false);
        StartCoroutine(Cooldown(slashCooldown));
    } //When the player can use the dash again

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != player && other.tag == "Player")
        {
            other.GetComponent<PlayerBody>().damagePlayer(damage);
        }
        else if (other.tag == "Enemy")
        {
            other.GetComponent<TheLost>().DamageEnemy(damage);
        }
    }
}
