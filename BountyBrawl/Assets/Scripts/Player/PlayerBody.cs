using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerBody : MonoBehaviour
{
    private Transform spawnPoint;

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
    private bool square = false;
    private bool triangle = false;

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
    private bool dead; //If player is dead

    //Knockback
    private bool knockbacked; //If player is hit with knockback
    private float knockbackStrength; //Strength of knockback
    private Vector2 knockbackDir; //Direction of knockback

    //Railgun
    private bool railgun;

    //Money
    [Tooltip("% amount of money lost on player killed")]
    [SerializeField] private float moneyLost = 20;

    //Stat Tracking
    [HideInInspector] public StatTracker statTracker;

    [SerializeField] private Color poisoned;
    [SerializeField] private Color hit;

    private BoxCollider2D boxCollider;

    //Particles
    [SerializeField] private ParticleSystem goldDrop;
    [SerializeField] private ParticleSystem livesGot;
    [SerializeField] private ParticleSystem waterSplash;
    [SerializeField] private ParticleSystem poisonEffect;

    [SerializeField] AudioSource playerDamage;
    [SerializeField] AudioSource playerSteps;

    [HideInInspector] public float maxAmmo; //For the ammo slider to determine the max value of the slider
    [HideInInspector] public float currAmmo; //For the ammo slider to determine the current value of the slider

    //Player skin selection
    public int playerSkin;
    [SerializeField] private RuntimeAnimatorController[] skins;

    //Glue and water slowness
    private bool glued;
    private bool wet;

    private AnimatorOverrideController animatorOverrideController;
    //private AnimatorOverrideController animatorOverrideController;

    [SerializeField] private float moneyGive = 50f;
    private int moneyChecker;

    [SerializeField] private AudioClip normalWalking;
    [SerializeField] private AudioClip walkingWater;


    [HideInInspector] public bool canPause;


    [HideInInspector] public bool spawnImmunity;

    [HideInInspector] public bool trueDeath;

    private void Awake()
    {
        
        canPause = true;
        playerRB = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        headSprite = playerHead.GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        weapon = false;
        useDefault = false;
        canMove = true;
        hammer = false;
        tempSpeed = runSpeed;
        weaponIndex = 0;
        lives = 3;
        money = 0;
        knockbacked = false;
        railgun = false;
        glued = false;
        wet = false;
        moneyChecker = 1;
        spawnImmunity = false;

       GetComponent<Animator>().runtimeAnimatorController = skins[playerSkin];

        playerSteps.clip = normalWalking;

        trueDeath = false;


    }

    private void OnDisable()
    {
        PlayerBody[] players = FindObjectsOfType<PlayerBody>();
        if (players.Length != 0)
        {
            //Debug.Log("Player Length where not 0" + players.Length);
            statTracker.placement = players.Length + 1;
            statTracker.playerIndex = playerIndex;
        }
        else
        {
            //Debug.Log("Player Length where is 0" + players.Length);
            statTracker.placement = 1;
            statTracker.playerIndex = playerIndex;
        }
    }

    private void Start()
    {
        //Gets the spawnpoint of the player based on player index
        GameObject[] spawnPoints;
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoints");
        spawnPoint = spawnPoints[playerIndex].transform;

        transform.position = spawnPoint.position; //Moves player to the spawnpoint

        //Get the stat tracker
        StatTracker[] statTrackers = FindObjectsOfType<StatTracker>();
        statTracker = statTrackers[playerIndex];
        statTracker.Reset();
        statTracker.SetPlayer(this.GetComponent<PlayerBody>());
       
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
            animator.SetBool("Attacking", false);
            defaultWeapon.SetActive(false);
        } //Turns on or off hand depending on if the player has a weapon
        if (weapon == true)
        {
            defaultWeapon.SetActive(false);
        }

        if (dead)
        {
            canMove = false;
            playerRB.velocity = Vector2.zero;
            playerRB.angularVelocity = 0;
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
        if (respawn != 0)
        {
            lives = lives - 1;
        }


        if (dead)
        {
            playerRB.velocity = Vector2.zero;
            playerRB.angularVelocity = 0;
        }


        //Handles player movement
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

            if (animator != null)
            {
                if (inputVector.magnitude != 0 && !dead)
                {
                    animator.SetFloat("Run", Mathf.Abs(inputVector.magnitude * runSpeed));

                    if (wet && !waterSplash.isPlaying && !dead)
                    {
                        waterSplash.Play();
                        playerSteps.clip = walkingWater;
                        playerSteps.Play();
                    }else if(!wet && !dead && !playerSteps.isPlaying)
                    {
                        playerSteps.Play();
                    }
                }
                else
                {
                    animator.SetFloat("Run", 0f);
                    animator.SetTrigger("Idle");
                    playerSteps.Stop();

                    if (wet || dead)
                    {
                        waterSplash.Stop(true);
                    }
                }
            }


            Facing();
        }

        if (knockbacked)
        {
            playerRB.velocity = knockbackDir.normalized * knockbackStrength;
        }

        //GameObject.Find("UI").GetComponent<FindPause>().GetPauseMenu() != null

        if (pause == true)
        {
            Debug.Log("pause");
            GameObject.Find("UI").GetComponent<PauseScript>().PressedPause();
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
                if (!railgun) //weaponHolder rotation if the player is not using railgun
                {
                    weaponHolder.rotation = Quaternion.Euler(0f, 180, -angle); //Rotates weapon around player
                }
                else //Slow weaponHolder rotation if the player is using railgun
                {
                    Quaternion weaponHolderRot = Quaternion.Euler(0f, 180, -angle);
                    weaponHolder.rotation = Quaternion.RotateTowards(weaponHolder.rotation, weaponHolderRot, Time.deltaTime * 25f);
                }

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
                if (!railgun) //weaponHolder rotation if the player is not using railgun
                {
                    weaponHolder.rotation = Quaternion.Euler(0f, 0f, angle); //Rotates weapon around player
                }
                else //Slow weaponHolder rotation if the player is using railgun
                {
                    Quaternion weaponHolderRot = Quaternion.Euler(0f, 0f, angle);
                    weaponHolder.rotation = Quaternion.RotateTowards(weaponHolder.rotation, weaponHolderRot, Time.deltaTime * 25f);
                }
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
        sprite.material.color = poisoned;
        headSprite.material.color = poisoned;
        poisonEffect.Play();
        yield return new WaitForSeconds(interval);

        //Goes through each amount of poison and damages the player
        for (int i = 0; i <= amount - 1; i++)
        {
            damagePlayer(dam, player);
            yield return new WaitForSeconds(interval); //Wait for the next poison interval
        }

        sprite.material.color = Color.white;
        headSprite.material.color = Color.white;
        poisonEffect.Stop();

        poison = null;

    }

    public IEnumerator Stun(float length)
    {
        canMove = false;
        playerRB.velocity = Vector2.zero;
        yield return new WaitForSeconds(length);
        canMove = true;
    }

    public IEnumerator Knocked(float strength, Vector2 dir)
    {
        knockbackStrength = strength;
        knockbacked = true;
        knockbackDir = dir;
        yield return new WaitForSeconds(0.1f);
        knockbacked = false;
    }

    //slows player in glue
    public void Slow(float slowness) {
        if (!glued)
        {
            glued = true;
            runSpeed -= slowness;
        }
    }
    //Fixes runspeed after player leaves glue
    public void ExitGlue(float slowness) {
        if (glued)
        {
            glued = false;
            runSpeed += slowness;
        }
    }

    //slow player if using a railgun
    public void Railgun(float slowness)
    {
        runSpeed -= slowness;
    }
    public void ExitRailgun(float slowness)
    {
        runSpeed += slowness;
    }

    public void EnterWater(float slowness)
    {
        if (!wet)
        {
            wet = true;
            runSpeed -= slowness;
        }
    }
    public void ExitWater(float slowness)
    {
        if (wet)
        {
            playerSteps.clip = normalWalking;
            wet = false;
            runSpeed += slowness;
            waterSplash.Stop(false);
        }
    }

    public void damagePlayer(float damage, PlayerBody player)
    {
        if (!dead)
        {
            StartCoroutine(Hit());
            if (playerDamage != null) { playerDamage.Play(); } //play damage sound if there is one
            health -= damage;

            if (player != null)
            {
                player.statTracker.IncreasePlayerDamage(damage);
            }

            if(health <= 0 && weaponHolder.gameObject.activeSelf && moneyChecker == 1)
            {
                moneyChecker--;
                boxCollider.enabled = false;
                dead = true;
                poisonEffect.Stop();
                spawnImmunity = true;

                if (player != null)
                {
                    player.IncreaseMoney(moneyGive);
                    player.statTracker.IncreasePlayerKills();
                }


                if (cursed)
                {
                    lives--;
                }

                StartCoroutine(Death());

            }
        }
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(0.1f);
        StopAllCoroutines();
        canMove = false;
        playerRB.velocity = Vector2.zero;
        sprite.material.color = Color.white;
        headSprite.material.color = Color.white;

        if (lives > 0)
        {
            animator.SetTrigger("Death");
        }
        else
        {
            animator.SetTrigger("TrueDeath");

        }
        animator.SetBool("Attacking", false);
        weaponHolder.gameObject.SetActive(false);
        headSprite.gameObject.SetActive(false);

        if(goldDrop != null)
        {
            goldDrop.Play();
        }
    }

    //player is hit by something
    private IEnumerator Hit()
    {

        if (!dead)
        {
            sprite.material.color = hit;
            headSprite.material.color = hit;
            yield return new WaitForSeconds(0.5f);
            if (poison == null)
            {
                sprite.material.color = Color.white;
                headSprite.material.color = Color.white;
            }
            else
            {
                sprite.material.color = poisoned;
                headSprite.material.color = poisoned;
            }
        }
    }

    //Moves player to respawn poisition and respawns if they have lives
    private void MoveToRespawn()
    {
        transform.position = spawnPoint.position;

        if (lives > 0)
        {
            animator.SetBool("Respawn", true);

        }
        else
        {

            GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().DeadPlayer(playerIndex);
            GameObject.Find("MainSceneLoader").GetComponent<SceneLoader>().CheckAmountRemaining();
            trueDeath = true;
            gameObject.SetActive(false);
        }
    }

    //Respawns player with stats
    private void Respawn()
    {
        dead = false;
        spawnImmunity = false;
        boxCollider.enabled = true;
        sprite.material.color = Color.white;
        headSprite.material.color = Color.white;
        animator.SetBool("Respawn", false);
        weaponHolder.gameObject.SetActive(true);
        headSprite.gameObject.SetActive(true);
        transform.position = spawnPoint.position;
        runSpeed = tempSpeed;
        poison = null;
        poisonEffect.Stop();
        knockbacked = false;
        weapon = false;
        useDefault = false;
        canMove = true;
        hammer = false;
        weaponIndex = 0;
        health = 100;
        moneyChecker = 1;
    }

    public void StartAttack()
    {
        //If player is not sandstorm then get rid of the head
        if (character != 0)
        {
            headSprite.gameObject.SetActive(false);
        }
        animator.SetBool("Attacking", true);
    }

    public void EndAttack()
    {
        animator.SetBool("Attacking", false);
        animator.SetTrigger("Idle");
        headSprite.gameObject.SetActive(true);
    }

    public void PoisonPlayer(float dam, float interval, float amount, PlayerBody player)
    {
        if (!dead && poison == null)
        {
            poison = StartCoroutine(Poison(dam, interval, amount, player));
        }
    }

    public Coroutine IsPoisoned() { return poison; }

    public void IncreaseMoney(float income) {
        if (lives < 3)
        {

            if (income > 0)
            {
                statTracker.IncreaseTotalMoney(income);
            }
            money += income;
        }
    }

    public void UsingRailgun(bool rail) { railgun = rail; }

    public float getFire1() { return fire1; } //Gets when the player inputs the primary fire
    public float getFire2() { return fire2; } //Gets when the player inputs the secondary fire
    public void ChangeMove(bool change) { canMove = change; } //Changes whether the player can move or not
    public bool UsingWeapon() { return weapon; } //If player is currently using a picked up weapon
    
    public void UsingDefault(bool def) { useDefault = def; } //Once the default weapon is fired
    public void ChangeWeapon(bool change) {
        headSprite.gameObject.SetActive(true);
        weapon = change; } //If player has picked up weapon

    public void EmeraldHammer(bool slam) { hammer = slam; } //Just for emeralds slam

    public float getThrow() { return nowThrow; } //Get if the player has chosen to throw his weapon

    public bool getPause() {return pause; } //Gets when the player inputs the pause button

    public bool getSquare() { return square; }

    public int getCharacter() { return character; }

    public void setWeaponIndex(int setWeaponIndex) { weaponIndex = setWeaponIndex; }

    public int getWeaponIndex() { return weaponIndex; }

    public float getMoney() { return money; }
    public float getHealth() { return health; }

    public int getLives() { return lives; }

    public void IncreaseLives() {
        livesGot.Play();
        lives++; }

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
        Debug.Log(pause);
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

    public void Square(bool squareButton)
    {
        square = squareButton;
        if (square == true)
        {
            if (playerSkin < 3)
            {
                
                playerSkin++;
                ChangeSkin();
            }
            else
            {
                playerSkin = 0;
                ChangeSkin();
            }
        }
    }

    public void Triangle(bool triangleButton)
    {
        triangle = triangleButton;
        if (triangle == true)
        {
            if (playerIndex == 0)
            {
                GameObject.FindGameObjectWithTag("P1").GetComponent<P1>().TriangleTrue();
            }
            if (playerIndex == 1)
            {
                GameObject.FindGameObjectWithTag("P2").GetComponent<P2>().TriangleTrue();
            }
            if (playerIndex == 2)
            {
                GameObject.FindGameObjectWithTag("P3").GetComponent<P3>().TriangleTrue();
            }
            if (playerIndex == 3)
            {
                GameObject.FindGameObjectWithTag("P4").GetComponent<P4>().TriangleTrue();
            }
        }
        else
        {
            if(playerIndex == 0)
            {
                GameObject.FindGameObjectWithTag("P1").GetComponent<P1>().TriangleFalse();
            }
            else if (playerIndex == 1)
            {
                GameObject.FindGameObjectWithTag("P2").GetComponent<P2>().TriangleFalse();
            }
            else if (playerIndex == 2)
            {
                GameObject.FindGameObjectWithTag("P3").GetComponent<P3>().TriangleFalse();
            }
            else if (playerIndex == 3)
            {
                GameObject.FindGameObjectWithTag("P4").GetComponent<P4>().TriasngleFalse();
            }
        }
    }


    public int GetPlayerIndex()
    {
        return playerIndex;
    } //Player index for multiple players

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }

    //Gets what character the player is playing
    public int GetPlayerCharacter()
    {
        return character;
    }

    public bool getDead() { return dead; }
    public void Curse() { cursed = true; }

    //for milestone demo
    public void ChangeSkin()
    {
        GetComponent<Animator>().runtimeAnimatorController = skins[playerSkin];
    }

    public int GetPlayerSkin()
    {
        return playerSkin;
    }

    public void SetPlayerSkin(int newSkin)
    {
        playerSkin = newSkin;
    }
}