using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Nagakome_Katana : MonoBehaviour
{

    [Tooltip("The time required before the slashes can be used again")]
    [SerializeField] private float slashCooldown = 3f;

    private BoxCollider2D attack;

    [SerializeField] private int animatorDefSpeed = 1;

    [SerializeField] private int animatorSlashSpeed = 2;

    [SerializeField] private PlayerBody player; //This is the player

    [SerializeField] private float damage = 9;

    private bool canFire = true; //Make sure the player can attack

    [SerializeField] private Animator animator;

    private Quaternion defRotation;

    [SerializeField] private AudioSource swinging;
    [SerializeField] private AudioClip[] soundClips;

    private float tempCooldown;

    private void Awake()
    {
        attack = GetComponent<BoxCollider2D>();
        animator.speed = animatorDefSpeed;
        defRotation = transform.rotation;
    }

    private void OnEnable()
    {
        player.UsingDefault(false);
        canFire = false;
        StartCoroutine(Cooldown(0.5f));
        animator.SetBool("Attack", false);
        animator.SetTrigger("Idle");
        animator.speed = animatorDefSpeed;
        transform.rotation = defRotation;
        animator.keepAnimatorControllerStateOnDisable = true;

        player.maxAmmo = slashCooldown;
        tempCooldown = slashCooldown;
        player.currAmmo = slashCooldown;
    }

    private void OnDisable()
    {
        animator.SetTrigger("Idle");
        attack.enabled = false;
        player.UsingDefault(false);
        StopAllCoroutines();
        animator.speed = animatorDefSpeed;
        transform.rotation = defRotation;
        animator.SetBool("Attack", false);

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

            if (player.maxAmmo > slashCooldown)
            {
                tempCooldown = slashCooldown;
                player.maxAmmo = slashCooldown;
            }

            if (tempCooldown < slashCooldown)
            {
                tempCooldown += Time.deltaTime;
                player.currAmmo = tempCooldown;
            }
        }
    }

    private void Shoot1()
    {
        animator.SetBool("Attack", true);
        canFire = false;
        attack.enabled = true;
        player.UsingDefault(true);
        animator.speed = animatorSlashSpeed;
    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canFire = true;
    } //When the player can use the dash again

    public void PlayRandSound()
    {
        int rand = (int)Random.Range(0f, soundClips.Length);
        swinging.clip = soundClips[rand];
        swinging.Play();
    }

    public void DoneSlashing()
    {
        player.UsingDefault(false);
        animator.SetBool("Attack", false);
        tempCooldown = 0;
        StartCoroutine(Cooldown(slashCooldown));
        animator.speed = animatorDefSpeed;
        attack.enabled = false;
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
