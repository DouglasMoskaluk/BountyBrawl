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

    [SerializeField] private float damage = 6f;

    [SerializeField] private PlayerBody player; //This is the player

    private bool canFire = true; //Make sure the player can dash
    private bool isDashing = false; //If the player is dashing

    private Vector2 traj; //Trajectory of the dash

    private BoxCollider2D attack;

    private void OnEnable()
    {
        attack = gameObject.GetComponent<BoxCollider2D>(); //Get the damage area
        player.UsingDefault(false);
        player.ChangeMove(true);
        attack.enabled = false;
        StopAllCoroutines();
        canFire = true;
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
        }

        if (isDashing)
        {
            player.gameObject.GetComponent<Rigidbody2D>().velocity = traj.normalized * dashSpeed;
            return;
        } //Moves the player in direction for dashing
    }

    private void Shoot1()
    {
        if (canFire)
        {
            attack.enabled = true;
            canFire = false;
            isDashing = true;
            player.UsingDefault(true);
            player.ChangeMove(false); //Player cannot move while dashing

            traj = player.GetFacing(); //Get the trajectory of the dash

            StartCoroutine(Dash(dashTime));
        } //If player can dash then do stop player from moving and 
    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canFire = true;
    } //When the player can use the dash again

    IEnumerator Dash(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(Cooldown(fistTime));
        isDashing = false;
        player.UsingDefault(false);
        player.ChangeMove(true);
        attack.enabled = false;
    } //When the player is done dashing

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != player && other.tag == "Player")
        {
            other.GetComponent<PlayerBody>().damagePlayer(damage);
        }else if(other.tag == "Enemy")
        {
            other.GetComponent<TheLost>().DamageEnemy(damage);
        }
    }
}
