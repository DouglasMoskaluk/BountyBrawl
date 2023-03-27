using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.UI;

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

    [SerializeField] private float moneyEarn = 100;

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

    [SerializeField] private float healthHeight = 5f;
    [SerializeField] private float healthSize = 0.3f;
    private Slider slider;
    private GameObject health;

    Path path;
    private int currWaypoint = 0;
    private bool reachedEndOfPath = false;

    Seeker seeker;

    private float tempDefSpeed;
    private bool canMove;

    CameraMovement_Big bigCam;
    CameraMovement_Small smallCam;

    private Coroutine poison;

    [SerializeField] private Color poisoned;
    [SerializeField] private Color hit;

    //Glue and water slowness
    private bool glued;
    private bool wet;

    private Animator animator;
    [SerializeField] private ParticleSystem goldDrop;

    private void Awake()
    {
        players = FindObjectsOfType<PlayerBody>(); //Get players
        rb = GetComponent<Rigidbody2D>();
        currSprite = GetComponent<SpriteRenderer>();
        body = GetComponent<BoxCollider2D>();
        poisonArea.gameObject.SetActive(false);
        cam = GameObject.FindGameObjectWithTag("MainCamera"); //get main camera
        eventM = GameObject.FindGameObjectWithTag("GameController").GetComponent<EventManager>(); //Get the event manager
        animator = GetComponent<Animator>();
        harbringer = currSprite.sprite;

        //Deals with eater health
        health = GameObject.FindGameObjectWithTag("EaterHealth");
        slider = health.GetComponentInChildren<Slider>();
        health.SetActive(false);
        health.transform.position = Vector3.zero;
        health.transform.localScale *= healthSize;

        isMiniboss = false;
        tempTimer = minionSpawn; //Saves default time for minion spawn
        tempPoisonStart = poisonStartSize; //Saves default start size of poison area
        tempPoisonEnd = poisonEndSize; //Saves default end size of poison area

        seeker = GetComponent<Seeker>();

        //Finds closest player to chase
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void OnEnable()
    {
        currSprite.color = Color.white;
        poisonStartSize = tempPoisonStart;
        poisonEndSize = tempPoisonEnd;
        poisonArea.localScale = tempPoisonStart;
        currHealth = baseHealth; //Saves the start health for when it is scaled up
        currDamage = baseDamage; //Saves the start damage for when it is scaled up
        health.SetActive(false);
        body.enabled = false;
        glued = false;
        wet = false;

        //Adds the eater to the camera list 
        if (cam != null)
        {
            if (cam.GetComponent<CameraMovement_Big>().isActiveAndEnabled)
            {
                bigCam = cam.GetComponent<CameraMovement_Big>();
                bigCam.AddEater(this.gameObject);
            }
            else
            {
                smallCam = cam.GetComponent<CameraMovement_Small>();
                smallCam.AddEater(this.gameObject);
            }
        }
        canMove = true;
        tempDefSpeed = enemySpeed;
        poison = null;
        minionSpawn = tempTimer;

        animator.SetBool("Dead", false);
        animator.SetTrigger("Spawn");
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

        if (health != null && health.activeSelf)
        {
            health.SetActive(false);
        }
        eventM.MinibossDead();
        isMiniboss = false;
        currSprite.sprite = harbringer;
        body.isTrigger = true;
    }

    void UpdatePath()
    {
        distance = float.PositiveInfinity;

        foreach (var player in players)
        {
            if (player.isActiveAndEnabled)
            {
                float dist = Vector3.Distance(player.transform.position, transform.position);

                if (dist < distance)
                {
                    distance = dist;
                    target = player.gameObject;
                }
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

            if(poison != null)
            {
                currSprite.color = poisoned;
            }

            //Death
            if (currHealth <= 0f)
            {
                goldDrop.Play();
                body.enabled = false;
                rb.velocity = Vector2.zero;
                animator.SetBool("Dead", true);
            }

            slider.value = currHealth;

            Vector2 eaterPos = transform.position;

            health.transform.position = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y + healthHeight));
        }
    }
    private void FixedUpdate()
    {

        //Moves eater along the path
        if (target != null && isMiniboss && canMove)
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

            lost.AddDamage(eventM.GetDamageIncrease());
            lost.AddHealth(eventM.GetHealthIncrease());
        }
        numMinions++;
        eventM.IncreaseNumspawn();
    }

    public void IsMiniboss(){
        animator.SetTrigger("IsBoss");

    } //If the eater is now a miniboss

    public void DoneBoss()
    {
        poisonArea.GetComponent<PoisonArea>().setDamage(currDamage);
        body.isTrigger = false;
        body.enabled = true;
        currSprite.sprite = boss;
        isMiniboss = true;
        poisonArea.gameObject.SetActive(true);
        health.SetActive(true);
        animator.SetTrigger("Chasing");
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currWaypoint = 0;
        }
    }

    public void Death()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator Poison(float dam, float interval, float amount, PlayerBody player)
    {
        yield return new WaitForSeconds(interval);

        //Goes through each amount of poison and damages the player
        for (int i = 0; i <= amount - 1; i++)
        {
            DamageEnemy(dam, player);
            yield return new WaitForSeconds(interval); //Wait for the next poison interval
        }

        poison = null;

        currSprite.color = Color.white;

    }

    public void PoisonEater(float dam, float interval, float amount, PlayerBody player)
    {
        poison = StartCoroutine(Poison(dam, interval, amount, player));
    }

    public IEnumerator Stun(float length)
    {
        canMove = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(length);
        canMove = true;
    }

    //slows Lost in glue
    public void Slow(float slowness)
    {
        if (!glued)
        {
            glued = true;
            enemySpeed -= slowness;
        }
    }

    //Fixes runspeed after player leaves glue
    public void ExitGlue(float slowness)
    {
        if (glued)
        {
            glued = false;
            enemySpeed += slowness;
        }
    }

    public void EnterWater(float slowness)
    {
        if (!wet)
        {
            wet = true;
            enemySpeed -= slowness;
        }
    }
    public void ExitWater(float slowness)
    {
        if (wet)
        {
            wet = false;
            enemySpeed += slowness;
        }
    }

    public void DamageEnemy(float damage, PlayerBody player) 
    { 
        if (isMiniboss) 
        {
            player.gameObject.GetComponent<StatTracker>().IncreaseEnemyDamage(damage);
            currHealth -= damage;
            StartCoroutine(HitColor());

            if(currHealth <= 0)
            {
                StopAllCoroutines();
                currSprite.color = Color.white;
                player.gameObject.GetComponent<StatTracker>().IncreaseEaterKills();
                player.IncreaseMoney(moneyEarn);
            }
        } 
    }

    private IEnumerator HitColor()
    {
        currSprite.color = hit;
        yield return new WaitForSeconds(0.5f);
        currSprite.color = Color.white;
    }

    public void IsTeleporting() 
    {
        if(bigCam != null)
        {
            bigCam.EaterIsTeleporting();
        }
    }

    public void IsNotTeleporting() 
    {
        if (bigCam != null)
        {
            bigCam.EaterIsNotTeleporting();
        }
    }

    public Coroutine IsPoisoned() { return poison; }
}
