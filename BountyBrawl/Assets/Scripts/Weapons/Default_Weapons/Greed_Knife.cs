using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Greed_Knife : MonoBehaviour
{

    [Tooltip("The time required before the slashes can be used again")]
    [SerializeField] private float stabCooldown = 1.5f;

    [Tooltip("Speed at which the slashes happen")]
    [SerializeField] private float slashSpeed = 0.3f;

    [SerializeField] private float damage = 2f;

    private BoxCollider2D attack;

    [SerializeField] private PlayerBody player; //This is the player

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
        yield return new WaitForSeconds(time);
        attack.enabled = true;
        yield return new WaitForSeconds(0.05f);
        attack.enabled = false;
        yield return new WaitForSeconds(time);
        attack.enabled = true;
        yield return new WaitForSeconds(0.05f);
        attack.enabled = false;
        player.UsingDefault(false);
        StartCoroutine(Cooldown(stabCooldown));
    } //When the player can use the dash again

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != player && collision.transform.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerBody>().damagePlayer(damage);
        }
    }
}
