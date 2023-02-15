using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Throw : MonoBehaviour
{
    [SerializeField] private float throwSpeed = 60f;
    [SerializeField] private float throwTime = 0.2f; //How long the weapon will be thrown for
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float throwDamage = 5f;
    [SerializeField] private float knockbackStrength = 40f;
    private bool isThrown = false;

    private Vector2 traj;

    private GameObject player; //The player that threw the weapon


    // Update is called once per frame
    void FixedUpdate()
    {
        if (isThrown)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = traj.normalized * throwSpeed;
            transform.Rotate(new Vector3(0f, 0f, -rotationSpeed * Time.deltaTime));
            return;
        }
    }

    public IEnumerator Cooldown(Vector2 direction, GameObject play)
    {
        player = play;
        isThrown = true;
        traj = direction;
        transform.position = (Vector2)transform.position + traj.normalized * 2;
        yield return new WaitForSeconds(throwTime);
        isThrown = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    } //When the weapon has done being thrown


    //collision detection for when it hits a wall and player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //If the weapon collides with a player other than the player that threw the weapon
        if (collision.gameObject != player && collision.transform.tag == "Player")
        {
            PlayerBody enemy = collision.transform.GetComponent<PlayerBody>();
            isThrown = false;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            enemy.damagePlayer(throwDamage, player.GetComponent<PlayerBody>());
            enemy.StartCoroutine(enemy.Knocked(knockbackStrength, traj.normalized));
            GetComponent<BoxCollider2D>().isTrigger = true;
            StopAllCoroutines();

            //If hit wall then stop weapon
        }
        else if(collision.transform.tag == "Barrier" || collision.transform.tag == "Wall")
        {
            isThrown = false;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StopAllCoroutines();

        //If collide with lost then damage and stop
        }else if(collision.transform.tag == "Lost")
        {
            isThrown = false;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StopAllCoroutines();
            collision.transform.GetComponent<TheLost>().DamageEnemy(throwDamage, player.GetComponent<PlayerBody>());
        }
        //If collide with eater then damage and stop
        else if (collision.transform.tag == "Eater")
        {
            isThrown = false;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StopAllCoroutines();
            collision.transform.GetComponent<TheEater>().DamageEnemy(throwDamage, player.GetComponent<PlayerBody>());
        }

    }
}