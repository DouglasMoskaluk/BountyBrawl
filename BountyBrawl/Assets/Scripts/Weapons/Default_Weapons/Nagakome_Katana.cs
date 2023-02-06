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

    [SerializeField] private Animator animator;


    private void Awake()
    {
        attack = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        canFire = true;
        animator.SetTrigger("Idle");
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

            if(player.getInputVector() != Vector2.zero)
            {
                animator.SetTrigger("Run");
            }
            else
            {
                animator.SetTrigger("Idle");
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
        animator.SetBool("Attack", true);
        attack.enabled = true;
        yield return new WaitForSeconds(0.05f);
        attack.enabled = false;
        yield return new WaitForSeconds(time);
        attack.enabled = true;
        yield return new WaitForSeconds(0.05f);
        attack.enabled = false;
        player.UsingDefault(false);
        StartCoroutine(Cooldown(slashCooldown));
        animator.SetBool("Attack", false);
    } //When the player can use the dash again

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != player && other.tag == "Player")
        {
            other.GetComponent<PlayerBody>().damagePlayer(damage);
        }
        if (other.tag == "Lost")
        {
            other.GetComponent<TheLost>().DamageEnemy(damage);
        }
        if (other.tag == "Eater")
        {
            other.GetComponent<TheEater>().DamageEnemy(damage);
        }
    }
}
