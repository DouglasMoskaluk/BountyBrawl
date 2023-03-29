using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sandstorm_Fist : MonoBehaviour
{
    [Tooltip("The time required before fists can be used again")]
    [SerializeField] private float fistTime = 4f;

    [Tooltip("How long the character will dash")]
    [SerializeField] private float dashTime = 1f;

    [Tooltip("How fast the character will dash")]
    [SerializeField] private float dashSpeed = 60f;

    [SerializeField] private AnimationCurve dashCurve;

    [SerializeField] private float damage = 6f;

    [SerializeField] private PlayerBody player; //This is the player

    private bool canFire = true; //Make sure the player can dash
    private bool isDashing = false; //If the player is dashing

    private Vector2 traj; //Trajectory of the dash

    private BoxCollider2D attack;

    [SerializeField] private AudioSource dash;

    private float tempCooldown;

    private void OnEnable()
    {
        attack = gameObject.GetComponent<BoxCollider2D>(); //Get the damage area
        player.UsingDefault(false);
        player.ChangeMove(true);
        attack.enabled = false;
        StopAllCoroutines();
        canFire = false;
        StartCoroutine(Cooldown(0.5f));

        player.maxAmmo = fistTime;
        tempCooldown = fistTime;
        player.currAmmo = fistTime;
    }

    private void OnDisable()
    {
        player.ChangeMove(true);
    } //Once the fists are disabled, let player move again.

    private void Update()
    {
        if (player != null)
        {
            if (player.getFire1() != 0)
            {
                Shoot1();
            }

            if(player.maxAmmo > fistTime)
            {
                tempCooldown = fistTime;
                player.maxAmmo = fistTime;
            }

            if (tempCooldown < fistTime)
            {
                tempCooldown += Time.deltaTime;
                player.currAmmo = tempCooldown;
            }
        }

        if (isDashing)
        {
            player.gameObject.GetComponent<Rigidbody2D>().velocity = traj.normalized * dashSpeed;
            return;
        } //Moves the player in direction for dashing
    }

    private void Shoot1()
    {
        if (canFire && player.getLastFacing() != Vector2.zero && isActiveAndEnabled)
        {
            dash.Play();
            attack.enabled = true;
            canFire = false;
            isDashing = true;
            player.UsingDefault(true);

            traj = player.getLastFacing(); //Get the trajectory of the dash

            player.StartAttack();
            StartCoroutine(Dash(dashTime));
        } //If player can dash then do stop player from moving and 
    }

    IEnumerator Cooldown(float time)
    {
        tempCooldown = 0;
        yield return new WaitForSeconds(time);
        canFire = true;
    } //When the player can use the dash again

    IEnumerator Dash(float time)
    {
        yield return new WaitForSeconds(0.1f);
        player.ChangeMove(false); //Player cannot move while dashing
        yield return new WaitForSeconds(time);
        StartCoroutine(Cooldown(fistTime));
        player.EndAttack();
        isDashing = false;
        player.UsingDefault(false);
        player.ChangeMove(true);
        attack.enabled = false;
    } //When the player is done dashing

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
        if(other.tag == "Box")
        {
            StartCoroutine(Cooldown(fistTime));
            player.EndAttack();
            isDashing = false;
            player.UsingDefault(false);
            player.ChangeMove(true);
            attack.enabled = false;

            WeaponBox box = other.GetComponent<WeaponBox>();
            player.IncreaseMoney(box.GetMoney());
            
        }
    }
}
