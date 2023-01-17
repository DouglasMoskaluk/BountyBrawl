using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private PlayerBody[] players;
    private float distance;
    private GameObject target;

    [SerializeField] private float enemySpeed = 2f;

    [SerializeField] private float baseDamage = 5f;

    [SerializeField] private float baseHealth = 30f;

    [SerializeField] private float currDamage;
    [SerializeField] private float currHealth;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        players = FindObjectsOfType<PlayerBody>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {


        distance = float.PositiveInfinity;

        foreach(var player in players)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);

            if(dist < distance)
            {
                distance = dist;
                target = player.gameObject;
            }
        }

        if(currHealth <= 0f)
        {
            Death();
        }
        
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.Normalize();
            rb.MovePosition(transform.position + (direction * enemySpeed * Time.deltaTime));
        }
    }

    private void LateUpdate()
    {

        if (target != null)
        {
            Vector2 face = target.transform.position - transform.position; //Get 2d position of the player
            
            if(face.x > 0.1)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

    }

    private void Death()
    {
        Destroy(gameObject);
    }

    public void AddDamage(float add){ currDamage += add + baseDamage; } //Adds more damage each time enemy is spawned
    
    public void AddHealth(float add) { currHealth += add + baseHealth; } //Adds more health each time enemy is spawned

    public void DamageEnemy(float damage) { currHealth -= damage; }
}
