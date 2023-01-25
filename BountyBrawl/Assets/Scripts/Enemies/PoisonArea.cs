using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonArea : MonoBehaviour
{
    private float damage;

    [Tooltip("The amoung of time before the player can be damaged again")]
    [SerializeField] private float damageTickTime = 2f;

    private float tempTimer;

    private void OnEnable()
    {
        tempTimer = damageTickTime;
    }

    public void setDamage(float newDamage) { damage = newDamage; }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (damageTickTime > 0)
            {
                damageTickTime -= Time.deltaTime;
            }
            else
            {
                damageTickTime = tempTimer;
                collision.GetComponent<PlayerBody>().damagePlayer(damage);
            }
        }
    }
}
