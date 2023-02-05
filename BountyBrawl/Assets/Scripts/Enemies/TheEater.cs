using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class TheEater : MonoBehaviour
{

    private PlayerBody[] players;
    private float distance;
    private GameObject target;

    [SerializeField] private Sprite boss;

    [SerializeField] private int numMinions = 4;

    [SerializeField] private float enemySpeed = 500f;

    [SerializeField] private float baseDamage = 20f;

    [SerializeField] private float baseHealth = 500f;

    [SerializeField] private float minionSpawn = 30f;

    [SerializeField] Transform poisonArea;

    [SerializeField] Vector3 poisonStartSize = new Vector3(0f, 0f, 0f);

    [SerializeField] Vector3 poisonEndSize = new Vector3(1f, 1f, 1f);

    [SerializeField] float speedOfPoisonGrowth = 0.5f;

    private EventManager eventM;

    private GameObject cam;

    private bool isMiniboss = false;

    private SpriteRenderer currSprite;
    private BoxCollider2D body;

    private float currDamage;
    [SerializeField] private float currHealth;
    private Vector3 tempPoisonStart;
    private Vector3 tempPoisonEnd;

    private float tempTimer;

    private Rigidbody2D rb;

    private Sprite harbringer;


    [Tooltip("How close enemy needs to be from waypoint before creating a new path")]
    [SerializeField] private float nextWayPointDistance = 10f;

    Path path;
    private int currWaypoint = 0;
    private bool reachedEndOfPath = false;

    Seeker seeker;

    private void OnEnable()
    {
        poisonStartSize = tempPoisonStart;
        poisonEndSize = tempPoisonEnd;
        poisonArea.localScale = tempPoisonStart;
        currHealth = baseHealth; //Saves the start health for when it is scaled up
        currDamage = baseDamage; //Saves the start damage for when it is scaled up

        //Adds the eater to the camera list 
        if (cam != null)
        {
            if (cam.GetComponent<CameraMovement_Big>().isActiveAndEnabled)
            {
                cam.GetComponent<CameraMovement_Big>().AddEater(this.gameObject);
            }
            else
            {
                cam.GetComponent<CameraMovement_Small>().AddEater(this.gameObject);
            }
        }
    }

    //Replace with onDisable later on
    private void OnDisable()
    {
        //deletes the eater form the list 
        if (cam != null)
        {
            if (cam.GetComponent<CameraMovement_Big>().isActiveAndEnabled)
            {
                cam.GetComponent<CameraMovement_Big>().DeleteEater();
            }
            else
            {
                cam.GetComponent<CameraMovement_Small>().DeleteEater();
            }
        }

        eventM.MinibossDead();
        isMiniboss = false;
        currSprite.sprite = harbringer;
    }

    private void Awake()
    {
        players = FindObjectsOfType<PlayerBody>(); //Get players
        rb = GetComponent<Rigidbody2D>();
        currSprite = GetComponent<SpriteRenderer>();
        body = GetComponent<BoxCollider2D>();
        poisonArea.gameObject.SetActive(false);
        cam = GameObject.FindGameObjectWithTag("MainCamera"); //get main camera
        eventM = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>(); //Get the event manager
        harbringer = currSprite.sprite;

        isMiniboss = false;
        tempTimer = minionSpawn; //Saves default time for minion spawn
        tempPoisonStart = poisonStartSize; //Saves default start size of poison area
        tempPoisonEnd = poisonEndSize; //Saves default end size of poison area

        seeker = GetComponent<Seeker>();

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


    void Update()
    {
        if (isMiniboss)
        {
            //Spawn minions
            if (minionSpawn > 0)
            {
                minionSpawn -= Time.deltaTime;
            }
            else
            {
                minionSpawn = tempTimer;
                SpawnMinions();
            }

            //Find closest player
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

            //changes poison area size
            if (isMiniboss)
            {
                //Change size of poison area over time
                if (poisonArea.localScale.x < poisonEndSize.x)
                {
                    Vector3 temp = poisonArea.localScale;
                    temp.x += Time.deltaTime * speedOfPoisonGrowth;
                    temp.y += Time.deltaTime * speedOfPoisonGrowth;
                    temp.z += Time.deltaTime * speedOfPoisonGrowth;

                    poisonArea.transform.localScale = temp;

                }
            }

            //Death
            if (currHealth <= 0f)
            {
                Death();
            }

        }
    }
    private void FixedUpdate()
    {

        //Moves eater along the path
        if (target != null && isMiniboss)
        {
            if (path == null)
                return;

            if (currWaypoint >= path.vectorPath.Count)
            {
                reachedEndOfPath = true;
                return;
            }
            else
            {
                reachedEndOfPath = false;
            }

            //Moves enemy towards closest player
            if (target != null)
            {
                Vector2 direction = ((Vector2)path.vectorPath[currWaypoint] - rb.position).normalized;
                Vector2 force = direction * enemySpeed * Time.deltaTime;

                rb.AddForce(force);

                float dist = Vector2.Distance(rb.position, path.vectorPath[currWaypoint]);

                if (dist < nextWayPointDistance)
                {
                    currWaypoint++;
                }
            }
        }
    }

    private void LateUpdate()
    {
        //Face direction of player
        if (target != null)
        {
            Vector2 face = target.transform.position - transform.position; //Get 2d position of the player

            if (face.x > 0.1)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }

    }

    private void SpawnMinions()
    {
        //Spawns the number of minions as declared as numMinions
        for (int i = 0; i <= numMinions - 1; i++)
        {
            //gets the position for enemy spawn and makes sure enemies aren't stuck on eachother
            Vector3 spawn = new Vector3(transform.position.x + i / 1.2f, transform.position.y - i / 1.2f, 0f);
            TheLost lost = ObjectPooler.Instance.SpawnFromPool("Lost", spawn, Quaternion.identity).GetComponent<TheLost>();
        }
    }

    public void IsMiniboss(){
        poisonArea.GetComponent<PoisonArea>().setDamage(currDamage);
        body.isTrigger = false;
        currSprite.sprite = boss;
        isMiniboss = true;
        poisonArea.gameObject.SetActive(true);

    } //If the eater is now a miniboss

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

    public IEnumerator Poison(float dam, float interval, float amount)
    {

        yield return new WaitForSeconds(interval);

        //Goes through each amount of poison and damages the player
        for (int i = 0; i <= amount - 1; i++)
        {
            DamageEnemy(dam);
            yield return new WaitForSeconds(interval); //Wait for the next poison interval
        }

    }

    public void DamageEnemy(float damage) { if (isMiniboss) { currHealth -= damage; } }
}
