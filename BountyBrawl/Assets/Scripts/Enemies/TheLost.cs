using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TheLost : MonoBehaviour
{

    private PlayerBody[] players;
    private float distance;
    private GameObject target;

    [SerializeField] private float enemyDefaultSpeed = 600f;

    [SerializeField] private float enemyDashSpeed = 900f;

    [Tooltip("How close player must be for dashing")]
    [SerializeField] private float dashDistance = 15f;

    [SerializeField] private float baseDamage = 5f;

    [SerializeField] private float baseHealth = 30f;

    [SerializeField] private float dashTime = 3f;

    [SerializeField] private Sprite dash;

    [Tooltip("How close enemy needs to be from waypoint before creating a new path")]
    [SerializeField] private float nextWayPointDistance = 10f;

    Path path;
    private int currWaypoint = 0;
    private bool reachedEndOfPath = false;

    Seeker seeker;

    [SerializeField] private float currDamage;
    [SerializeField] private float currHealth;
    private bool canDash;

    private Rigidbody2D rb;

    private Sprite currSprite;
    private SpriteRenderer spriteRenderer;

    private void OnEnable()
    {
        currDamage = baseDamage;
        currHealth = baseHealth;
        canDash = true;

        spriteRenderer.sprite = currSprite;
    }

    private void Awake()
    {
        players = FindObjectsOfType<PlayerBody>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        currSprite = spriteRenderer.sprite;

        //Finds closest player to chase
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void UpdatePath() 
    {
        distance = float.PositiveInfinity;

        foreach (var player in players)
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);

            if (dist < distance)
            {
                distance = dist;
                target = player.gameObject;
            }
        }
        seeker.StartPath(rb.position, target.transform.position, OnPathComplete);
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
        if (path == null)
            return;

        if(currWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        //Moves enemy towards closest player by moving on the path
        if (target != null)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currWaypoint] - rb.position).normalized;

            Vector2 force = Vector2.zero;

            if(distance < dashDistance && canDash)
            {
                spriteRenderer.sprite = dash;
                force = direction * enemyDashSpeed * Time.deltaTime;
            }
            else
            {
                spriteRenderer.sprite = currSprite;
                force = direction * enemyDefaultSpeed * Time.deltaTime;
            }


            rb.AddForce(force);

            float dist = Vector2.Distance(rb.position, path.vectorPath[currWaypoint]);

            if(dist < nextWayPointDistance)
            {
                currWaypoint++;
            }
        }
    }

    private void LateUpdate()
    {
        //Moves the facing direction
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

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currWaypoint = 0;
        }
    }

    private void Death()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player" && canDash)
        {
            collision.transform.GetComponent<PlayerBody>().damagePlayer(currDamage);
            canDash = false;
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(dashTime);
        canDash = true;
    } //When the lost can dash into players and deal damage

    public void AddDamage(float add){ currDamage += add; } //Adds more damage each time enemy is spawned
    
    public void AddHealth(float add) { currHealth += add; } //Adds more health each time enemy is spawned

    public void DamageEnemy(float damage) { currHealth -= damage; }
}
