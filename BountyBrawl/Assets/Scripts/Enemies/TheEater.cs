using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEater : MonoBehaviour
{

    private PlayerBody[] players;
    private float distance;
    private GameObject target;

    [SerializeField] private GameObject minion;

    [SerializeField] private Sprite boss;

    [SerializeField] private int numMinions = 4;

    [SerializeField] private float enemySpeed = 4f;

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
            if (cam.GetComponent<CameraMovement_Big>() != null)
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
            if (cam.GetComponent<CameraMovement_Big>() != null)
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

            //Death
            if (currHealth <= 0f)
            {
                Death();
            }

        }
    }
    private void FixedUpdate()
    {
        if (target != null && isMiniboss)
        {
            Vector3 direction = target.transform.position - transform.position;
            direction.Normalize();
            rb.MovePosition(transform.position + (direction * enemySpeed * Time.deltaTime));
        }

        if (isMiniboss)
        {
            //Change size of poison area
            if (poisonArea.localScale.x < poisonEndSize.x) {
                Vector3 temp = poisonArea.localScale;
                temp.x += Time.deltaTime * speedOfPoisonGrowth;
                temp.y += Time.deltaTime * speedOfPoisonGrowth; ;
                temp.z += Time.deltaTime * speedOfPoisonGrowth; ;

                poisonArea.transform.localScale = temp;

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

    private void Death()
    {
        gameObject.SetActive(false);
    }

    public void DamageEnemy(float damage) { if (isMiniboss) { currHealth -= damage; } }
}
