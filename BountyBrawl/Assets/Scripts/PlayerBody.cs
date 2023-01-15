using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerBody : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Manipulates the speed of the player")] private float moveSpeed = 3f;
    [SerializeField] private int health = 100; // health of player
    private Rigidbody2D playerRB;

    private Vector2 inputVector = Vector2.zero; //Direction for movement
    private Vector2 facingVector = Vector2.zero; //Direction for where the gun should face
    private Vector2 lastFacing; //Last trajectory of the player before letting go of right stick
    private float fire1 = 0f; //Players primary fire input
    private float fire2 = 0f; //Players secondary fire input
    private bool canMove; //Whether the player can move or not
    private float nowThrow = 0f; //When the player has chosen to throw his weapon
    [SerializeField] private Transform weaponHolder; //The thing holding the weapon
    [SerializeField] private Transform playerHead; //The head of the player
    [SerializeField] private GameObject defaultWeapon; //The default weapon of the player
    private bool weapon; //If player is using a pickupable weapon
    private bool useDefault; //If player has fired their default weapon
    private bool hammer; //Specifically for Emerald when she is using her hammer
    [SerializeField] private int playerIndex = 0;
    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        weapon = false;
        useDefault = false;
        canMove = true;
        hammer = false;
    }
    private void Update()
    {
        //Switch with default weapon instead of hand
        if (weapon == false)
        {
            defaultWeapon.SetActive(true);
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
        if (canMove)
        {
            inputVector = inputVector.normalized;
            playerRB.velocity = inputVector * moveSpeed;
            Facing();
        }

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
            else if(!useDefault || !hammer)
            {
                weaponHolder.rotation = Quaternion.Euler(0f, 180, 0f);
            }


            if (angle < 20f && angle > -40f)
            {
                playerHead.rotation = Quaternion.Euler(0f, 180, -angle);
            }
            else if (angle >= 20f)
            {
                playerHead.rotation = Quaternion.Euler(0f, 180, -20);
            }
            else if (angle <= -40f)
            {
                playerHead.rotation = Quaternion.Euler(0f, 180, 40);
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
            else if(!useDefault && !hammer)
            {
                weaponHolder.rotation = Quaternion.Euler(0f, 0f, 0f);
            }

            if (angle < 40f && angle > -20f)
            {
                playerHead.rotation = Quaternion.Euler(0f, 0f, angle);
            }
            else if (angle >= 40f)
            {
                playerHead.rotation = Quaternion.Euler(0f, 0f, 40);
            }
            else if (angle <= -20f)
            {
                playerHead.rotation = Quaternion.Euler(0f, 0f, -20);
            }

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    } //Facing
    public float getFire1() { return fire1; } //Gets when the player inputs the primary fire
    public void ChangeMove(bool change) { canMove = change; } //Changes whether the player can move or not
    public bool UsingWeapon() { return weapon; } //If player is currently using a picked up weapon
    
    public void UsingDefault(bool def) { useDefault = def; } //Once the default weapon is fired
    public void ChangeWeapon(bool change) { weapon = change; } //If player has picked up weapon

    public void EmeraldHammer(bool slam) { hammer = slam; } //Just for emeralds slam

    public void damagePlayer(int damage)
    {
        health -= damage;
    }

    public float getThrow() { return nowThrow; } //Get if the player has chosen to throw his weapon

    public Vector2 getLastFacing() { return lastFacing; }

    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
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
    public int GetPlayerIndex()
    {
        return playerIndex;
    } //Player index for multiple players
}