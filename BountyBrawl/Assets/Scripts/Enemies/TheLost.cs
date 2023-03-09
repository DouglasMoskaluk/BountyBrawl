using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class TheLost : MonoBehaviour
{
    [System.Serializable]
    public class Type
    {
        public string name;
        public Sprite walkSprite;
        public Sprite runSprite;
        public string weaponDrop;
    }

    public List<Type> types;

    private PlayerBody[] players;
    private float distance;
    private GameObject target;

    [SerializeField] private float enemyDefaultSpeed = 600f;

    [SerializeField] private float enemyDashSpeed = 30f;

    [SerializeField] private float dropTypeChance = 40f; //The chance of the lost dropping the weapon type weapon
    [SerializeField] private float dontDropChance = 70f; //The chance of the lost dropping nothing
    [SerializeField] private float commonDropChance = 80f; //The chance of the lost dropping common weapon

    [Tooltip("How close player must be for dashing")]
    [SerializeField] private float dashDistance = 15f;

    [SerializeField] private float baseDamage = 5f;

    [SerializeField] private float baseHealth = 30f;

    [SerializeField] private float dashTime = 1f;

    [SerializeField] private Sprite dash;

    [SerializeField] private GameObject aura;

    [SerializeField] private float moneyEarn = 5;

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

    private bool canMove;

    private float tempDefSpeed;
    private float tempDashSpeed;

    private string lostTypeName;

    private Coroutine poison;

    private int weapon; //The weapon the lost will drop on death
    private string dropChanceWeapon; //The weapon the type of lost is highly likely to drop

    //For dashing
    private bool dashing;
    private Vector2 targetPos;

    private void Awake()
    {
        players = FindObjectsOfType<PlayerBody>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        tempDefSpeed = enemyDefaultSpeed;
        tempDashSpeed = enemyDashSpeed;

        //Finds closest player to chase
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void OnEnable()
    {
        currDamage = baseDamage;
        currHealth = baseHealth;
        canDash = true;
        dashing = false;
        canMove = true;
        aura.SetActive(false);

        int lostType = (int) Random.Range(0f, types.Count);

        lostTypeName = types[lostType].name;
        dropChanceWeapon = types[lostType].weaponDrop;
        currSprite = types[lostType].walkSprite;
        dash = types[lostType].runSprite;

        spriteRenderer.sprite = currSprite;
        poison = null;
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

    // Update is called once per frame
    void Update()
    {
        distance = float.PositiveInfinity;

        foreach(var player in players)
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
        if (target != null && canMove)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currWaypoint] - rb.position).normalized;

            Vector2 force = Vector2.zero;

            if(distance < dashDistance && canDash)
            {
                spriteRenderer.sprite = dash;
                targetPos = target.transform.position - transform.position;
                dashing = true;
                canDash = false;
                force = Vector2.zero;
                aura.SetActive(true);
                StartCoroutine(Cooldown());
            }
            else 
            {
                if (!dashing)
                {
                    aura.SetActive(false);
                    spriteRenderer.sprite = currSprite;
                    force = direction * enemyDefaultSpeed * Time.deltaTime;
                }
            }

            if (!dashing)
            {
                rb.AddForce(force);
            }
            else
            {
                rb.velocity = targetPos.normalized * enemyDashSpeed;
                return;
            }

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
        int notDrop = (int)Random.Range(1f, 101f);

        //If lost will drop weapon
        if (notDrop >= dontDropChance)
        {

            int chance = (int)Random.Range(1f, 101f);

            //drop common weapon
            if (chance <= commonDropChance)
            {
                weapon = (int)Random.Range(0f, 3f);
            }
            else //Drop rare weapon
            {
                weapon = (int)Random.Range(3f, 7f);
            }

            if (lostTypeName == "Default")
            {
                spawnWeapon();
            }
            else
            {
                int typeChance = (int)Random.Range(1f, 101f);
                
                //Dropping the higher chance weapon from the lost type
                if (typeChance <= dropTypeChance)
                {
                    ObjectPooler.Instance.SpawnFromPool(dropChanceWeapon, transform.position, transform.rotation);
                }
                else
                {
                    spawnWeapon();
                }
            }
        }

        gameObject.SetActive(false);
    }

    private void spawnWeapon()
    {
        if (weapon == 0) //Spawn common weapon 1
        {
            ObjectPooler.Instance.SpawnFromPool("SnakeBiteRevolver", transform.position, transform.rotation);
        }
        else if (weapon == 1) //Spawn common weapon 2
        {
            ObjectPooler.Instance.SpawnFromPool("WyldsnagShotgun", transform.position, transform.rotation);
        }
        else if (weapon == 2)//Spawn common weapon 3
        {
            ObjectPooler.Instance.SpawnFromPool("Ch-ChingRifle", transform.position, transform.rotation);
        }
        else if (weapon == 3) //Spawn rare weapon 1
        {
            ObjectPooler.Instance.SpawnFromPool("DeathWhisperShuriken", transform.position, transform.rotation);
        }
        else if (weapon == 4) //Spawn rare weapon 2
        {
            ObjectPooler.Instance.SpawnFromPool("DeathWhisperShuriken", transform.position, transform.rotation);
        }
        else //Spawn rare weapon 3
        {
            ObjectPooler.Instance.SpawnFromPool("Railgun", transform.position, transform.rotation);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.transform.tag == "Player" && dashing)
        {
            canDash = false;
            dashing = false;
            rb.velocity = Vector2.zero;
            collision.transform.GetComponent<PlayerBody>().damagePlayer(currDamage, null);
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(dashTime);
        dashing = false;
        yield return new WaitForSeconds(dashTime);
        canDash = true;
    } //When the lost can dash into players and deal damage

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

    }

    public void PoisonLost(float dam, float interval, float amount, PlayerBody player)
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
        enemyDefaultSpeed = slowness;
        enemyDashSpeed = slowness;
    }

    //Fixes runspeed after player leaves glue
    public void ExitGlue() 
    {
        enemyDefaultSpeed = tempDefSpeed;
        enemyDashSpeed = tempDashSpeed;
    }


    public void AddDamage(float add){ currDamage += add; } //Adds more damage each time enemy is spawned
    
    public void AddHealth(float add) { currHealth += add; } //Adds more health each time enemy is spawned

    public Coroutine IsPoisoned() { return poison; }

    public void DamageEnemy(float damage, PlayerBody player) 
    {
        player.gameObject.GetComponent<StatTracker>().IncreaseEnemyDamage(damage);

        currHealth -= damage;
        if (currHealth <= 0)
        {
            player.gameObject.GetComponent<StatTracker>().IncreaseLostKills();
            player.IncreaseMoney(moneyEarn);
        }
    }
}
