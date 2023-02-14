using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerBody : MonoBehaviour
{
    [SerializeField] private GameObject eventManager;
    GameObject UIPauseMenu; //Pause menu on canvas

    //Player info
    [SerializeField]
    [Tooltip("Manipulates the walk speed of the player")] private float walkSpeed = 5f;
    [SerializeField]
    [Tooltip("Manipulates the run speed of the player")] private float runSpeed = 9f;
    [SerializeField] private float health = 100; // health of player
    private Rigidbody2D playerRB;
    private SpriteRenderer sprite;
    private SpriteRenderer headSprite;
    private float money;

    //Stick inputs
    private Vector2 inputVector = Vector2.zero; //Direction for movement
    private Vector2 facingVector = Vector2.zero; //Direction for where the gun should face
    private Vector2 lastFacing; //Last trajectory of the player before letting go of right stick

    //Button inputs
    private float fire1 = 0f; //Players primary fire input
    private float fire2 = 0f; //Players secondary fire input
    private bool pause = false; //players pause input
    private bool canMove; //Whether the player can move or not
    private float nowThrow = 0f; //When the player has chosen to throw his weapon
    private float removeHealth = 0f;
    private float addHealth = 0f;
    private float respawn = 0f;

    //Serializables
    [SerializeField] private Transform weaponHolder; //The thing holding the weapon
    [SerializeField] private Transform playerHead; //The head of the player
    [SerializeField] private GameObject defaultWeapon; //The default weapon of the player
    [SerializeField] private float downAngle = 20f; // The down head angle of the player
    [SerializeField] private float upAngle = -40f; // The up head angle of the player

    //Default weapon checkers
    private bool weapon; //If player is using a pickupable weapon
    private bool useDefault; //If player has fired their default weapon
    private bool hammer; //Specifically for Emerald when she is using her hammer
    private Coroutine poison;

    public int weaponIndex;

    //Multiplayer player
    [SerializeField] private int playerIndex = 0; //Index for controllers
    [SerializeField] private int character; //Holds what type of character player is playing

    [SerializeField] private Animator animator;

    private float tempSpeed;
    private int lives;
    private bool cursed; //If the players will start having their lives

    private void Awake()
    {
        UIPauseMenu = GameObject.FindGameObjectWithTag("PauseMenu"); //finds pause menu ui
        playerRB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        headSprite = playerHead.GetComponent<SpriteRenderer>();
        weapon = false;
        useDefault = false;
        canMove = true;
        hammer = false;
        tempSpeed = runSpeed;
        weaponIndex = 0;
        lives = 3;;
        money = 0;
    }

    private void OnEnable()
    {
        poison = null;
    }

    private void Update()
    {
        //Switch with default weapon instead of hand
        if (weapon == false)
        {
            defaultWeapon.SetActive(true);
            setWeaponIndex(0);
        }
        else
        {
            defaultWeapon.SetActive(false);
        } //Turns on or off hand depending on if the player has a weapon
        if (weapon == true)
        {
            defaultWeapon.SetActive(false);
        }


        
    }
    private void FixedUpdate()
    {
        if (removeHealth != 0)
        {
            health = health - 1;
        }
        if (addHealth != 0)
        {
            health = health + 1;
        }
        if(respawn != 0)
        { 
           lives = lives - 1;
            
        }


        if (canMove)
        {

            if (inputVector.magnitude < 0.9f)
            {
                playerRB.velocity = inputVector.normalized * walkSpeed;
            }
            else if (inputVector.magnitude > 0.9f)
            {
                playerRB.velocity = inputVector.normalized * runSpeed;
            }
            
            if(animator != null)
            {
                if(inputVector.magnitude != 0)
                {
                    animator.SetFloat("Run", Mathf.Abs(inputVector.magnitude * runSpeed));
                }
                else
                {
                    animator.SetFloat("Run", 0f);
                    animator.SetTrigger("Idle");
                }
            }
            
            
            Facing();
        }

        

        if (getPause() == true && UIPauseMenu != null)
        {
            UIPauseMenu.GetComponent<PauseScript>().PressedPause();
        } //checks if player has pressed the pause menu button and toggles it

    }

    /*
     * This method moves the direction of the weapon and head of the player based on the right stick input
     */
    

    private void Facing()
    {
        //If the stick is moving in the left direction
        if (facingVector.x < -0.1)
        {
            float angle = facingVector.x + facingVector.y * -90;

            if (weapon || useDefault) //if using a pickupable weapon
            {
                weaponHolder.rotation = Quaternion.Euler(0f, 180, -angle); //Rotates weapon around player
            }
            else if (!useDefault || !hammer)
            {
                weaponHolder.rotation = Quaternion.Euler(0f, 180, 0f);
            }

            //Change head rotation
            if (angle < upAngle && angle > downAngle)
            {
                playerHead.rotation = Quaternion.Euler(0f, 180, -angle);
            }
            else if (angle >= upAngle)
            {
                playerHead.rotation = Quaternion.Euler(0f, 180, -upAngle);
            }
            else if (angle <= downAngle)
            {
                playerHead.rotation = Quaternion.Euler(0f, 180, -downAngle);
            }

            transform.rotation = Quaternion.Euler(0f, 180, 0f);
        }

        //If the stick is moving in the right direction
        else if (facingVector.x > 0.1)
        {
            float angle = facingVector.x + facingVector.y * 90;

            if (weapon || useDefault)
            {
                weaponHolder.rotation = Quaternion.Euler(0f, 0f, angle); //Rotates weapon around player
            }
            else if (!useDefault && !hammer)
            {
                weaponHolder.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            //Change head rotation
            if (angle < -downAngle && angle > -upAngle)
            {
                playerHead.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            else if (angle >= -downAngle)
            {
                playerHead.rotation = Quaternion.Euler(0f, 0f, -downAngle);
            }
            else if (angle <= -upAngle)
            {
                playerHead.rotation = Quaternion.Euler(0f, 0f, -upAngle);
            }

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    } //Facing

    public IEnumerator Poison(float dam, float interval, float amount, PlayerBody player)
    {

        yield return new WaitForSeconds(interval);

        //Goes through each amount of poison and damages the player
        for(int i = 0; i <= amount - 1; i++)
        {
            damagePlayer(dam, player);
            yield return new WaitForSeconds(interval); //Wait for the next poison interval
        }

        poison = null;

    }

    private IEnumerator Hit()
    {
        sprite.material.color = Color.red;
        headSprite.material.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        sprite.material.color = Color.white;
        headSprite.material.color = Color.white;
    }

    public IEnumerator Stun(float length)
    {
        canMove = false;
        playerRB.velocity = Vector2.zero;
        yield return new WaitForSeconds(length);
        canMove = true;
    }

    //slows player in glue
    public void Slow(float slowness){runSpeed = slowness;}

    //Fixes runspeed after player leaves glue
    public void ExitGlue() { runSpeed = tempSpeed; }

    public void damagePlayer(float damage, PlayerBody player)
    {
        StartCoroutine(Hit());
        health -= damage;
    }

    public void PoisonPlayer(float dam, float interval, float amount, PlayerBody player)
    {
        poison = StartCoroutine(Poison(dam, interval, amount, player));
    }

    public Coroutine IsPoisoned() { return poison; }

    public void IncreaseMoney(float income) { money += income; }

    public float getFire1() { return fire1; } //Gets when the player inputs the primary fire
    public float getFire2() { return fire2; } //Gets when the player inputs the secondary fire
    public void ChangeMove(bool change) { canMove = change; } //Changes whether the player can move or not
    public bool UsingWeapon() { return weapon; } //If player is currently using a picked up weapon

    
    public void UsingDefault(bool def) { useDefault = def; } //Once the default weapon is fired
    public void ChangeWeapon(bool change) { weapon = change; } //If player has picked up weapon

    public void EmeraldHammer(bool slam) { hammer = slam; } //Just for emeralds slam

    public float getThrow() { return nowThrow; } //Get if the player has chosen to throw his weapon

    public bool getPause() {return pause; } //Gets when the player inputs the pause button

    public int getCharacter() { return character; }

    public void setWeaponIndex(int setWeaponIndex) { weaponIndex = setWeaponIndex; }

    public int getWeaponIndex() { return weaponIndex; }

    public float getHealth() { return health; }

    public int getLives() { return lives; }

    public Vector2 getLastFacing() { return lastFacing; }

    public Vector2 getInputVector() { return inputVector; }

    public void SetInputVector(Vector2 direction)
    {
        if (Time.deltaTime != 0)
        {
            inputVector = direction;
        }
        else
        {
            inputVector = Vector2.zero;
        }
    } //For the left stick representing the direction of movement
    public void SetFacingVector(Vector2 face)
    {
        facingVector = face;
        if (facingVector != Vector2.zero)
        {
            lastFacing = facingVector;
        }
    } //For the right stick representing where to face

    public Vector2 GetFacing() { return facingVector; }
    public void Fire1(float click1)
    {
        fire1 = click1;
    } //Get players primary fire input
    public void Fire2(float click2)
    {
        fire2 = click2;
    } //Get players secondary fire input
    public void Throw(float circle)
    {
        nowThrow = circle;
    }
    public void Pause(bool start)
    {
        pause = start;
    }
    public void RemoveHealth(float down)
    {
        removeHealth = down;
    }
    public void AddHealth(float up)
    {
        addHealth = up;
    }
    public void Respawn(float left)
    {
        respawn = left;
    }

    public void ActivateEvent(float activateEvent){
        if (!eventManager.activeSelf)
        {
            eventManager.SetActive(true);
        }
        else
        {
            eventManager.SetActive(false);
        }
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    } //Player index for multiple players

    //Gets what character the player is playing
    public int GetPlayerCharacter()
    {
        return character;
    }

}