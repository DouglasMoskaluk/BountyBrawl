using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Emerald_Hammer : MonoBehaviour
{

    [Tooltip("The time required before the slam can be used again")]
    [SerializeField] private float slamCooldown = 5f;

    [Tooltip("Time for the slam to finish")]
    [SerializeField] private float slamSpeed = 1f;

    [Tooltip("Top angle of the hammer")]
    [SerializeField] private float topAngle = 140f;

    [Tooltip("Bottom angle of the hammer")]
    [SerializeField] private float bottomAngle = -45f;

    private CircleCollider2D attack;

    [SerializeField] private PlayerBody player; //This is the player

    [SerializeField] private Transform weaponHolder; //The thing that holds the weapon

    private bool canFire = true; //Make sure the player can attack

    private bool slamming = false; //Whether the player is slamming

    private float currAngle;
    private float currTime;

    private void Awake()
    {
        attack = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (player != null)
        {
            if (player.getFire1() != 0 && canFire)
            {
                StartCoroutine(Shoot1());
            }
        }

        //If the weapon is slamming
        if (slamming)
        {
            //Gets the time of the slam
            currTime += Time.deltaTime / slamSpeed;

            //If player is looking left
            if (player.getLastFacing().x < -0.1)
            {
                currAngle = Mathf.SmoothStep(-topAngle, -bottomAngle, currTime);
                currAngle = -currAngle;
                weaponHolder.rotation = Quaternion.Euler(0f, 180f, currAngle);
            }
            else //If player is looking right
            {
                currAngle = Mathf.SmoothStep(topAngle, bottomAngle, currTime);
                weaponHolder.rotation = Quaternion.Euler(0f, 0f, currAngle);
            }
        }
    }

    IEnumerator Shoot1()
    {
        canFire = false;
        slamming = true;
        player.EmeraldHammer(true);
        yield return StartCoroutine(Slam(slamSpeed)); //Wait till slam is over
        StartCoroutine(Cooldown(slamCooldown));
    }

    IEnumerator Slam(float time)
    {
        yield return new WaitForSeconds(time);
        //Slam is now over
        attack.enabled = true;

        yield return new WaitForSeconds(0.2f);
        attack.enabled = false;
        
        slamming = false;
        currTime = 0f;
        player.EmeraldHammer(false);

    } //When the player can use the dash again

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canFire = true;
    } //When the player can use the dash again

}
