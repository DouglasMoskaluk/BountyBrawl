using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Nagakome_Katana : MonoBehaviour
{

    [Tooltip("The time required before the slashes can be used again")]
    [SerializeField] private float slashCooldown = 3f;

    [Tooltip("Speed at which the slashes happen")]
    [SerializeField] private float slashSpeed = 0.5f;

    private BoxCollider2D attack;

    [SerializeField] private PlayerBody player; //This is the player

    private bool canFire = true; //Make sure the player can dash

    private void Awake()
    {
        attack = GetComponent<BoxCollider2D>();
    }

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
            canFire = false;

            StartCoroutine(Cooldown(slashCooldown));
            StartCoroutine(Slash(slashSpeed));
        } //If player can dash then do stop player from moving and 
    }

    IEnumerator Cooldown(float time)
    {
        yield return new WaitForSeconds(time);
        canFire = true;
    } //When the player can use the dash again

    IEnumerator Slash(float time)
    {
        attack.enabled = true;
        yield return new WaitForSeconds(time);
        attack.enabled = false;
        yield return new WaitForSeconds(time);
        attack.enabled = true;
        yield return new WaitForSeconds(time);
        attack.enabled = false;
    } //When the player can use the dash again
}
