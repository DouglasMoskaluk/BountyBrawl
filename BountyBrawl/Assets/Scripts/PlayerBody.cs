using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Manipulates the speed of the player")] private float moveSpeed = 3f;
    [SerializeField]
    private int health = 100; // health of player

    private Rigidbody2D playerRB;

    private Vector2 inputVector = Vector2.zero; //Direction for movement
    private Vector3 facingVector = Vector2.zero; //Direction for where the gun should face
    private float fire1 = 0f;
    private float fire2 = 0f;


    [SerializeField] private Transform weaponHolder; //The thing holding the weapon
    [SerializeField] private Transform playerHead; //The head of the player
    [SerializeField] private GameObject playerHand; //The hand of the player

    [HideInInspector]
    public bool weapon; //If player is using a pickupable weapon

    [SerializeField] private int playerIndex = 0; 

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        weapon = false;
    }

    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    } //For the left stick representing the direction of movement

    public void SetFacingVector(Vector2 face)
    {
        facingVector = face;
    } //For the right stick representing where to face

    public void Fire1(float click1)
    {
        fire1 = click1;
    }

    public void Fire2(float click2)
    {
        fire1 = click2;
    }

    public int GetPlayerIndex()
    {
        return playerIndex;
    } //Player index for multiple players

    private void Update()
    {
        //Switch with default weapon instead of hand
        if (weapon == false){
            playerHand.SetActive(true);
        }else{
            playerHand.SetActive(false);
        } //Turns on or off hand depending on if the player has a weapon

        if (weapon == true)
        {
            //weapon = GetComponentInChildren<PlayerWeapon>();
            playerHand.SetActive(false);
            //weapon.Shoot1(fire1);
        }

        Facing();

    }

    private void FixedUpdate()
    {
        inputVector = inputVector.normalized;

        playerRB.velocity = inputVector * moveSpeed;

    }

    private void Facing()
    {
        if (facingVector.x < -0.1)
        {
            float angle = facingVector.x + facingVector.y * -90;
            weaponHolder.rotation = Quaternion.Euler(0f, 180, -angle);

            if (angle < 20f && angle > -40f)
            {
                playerHead.rotation = Quaternion.Euler(0f, 180, -angle);
            }

            transform.rotation = Quaternion.Euler(0f, 180, 0f);
        }

        else if (facingVector.x > 0.1)
        {
            float angle = facingVector.x + facingVector.y * 90;
            weaponHolder.rotation = Quaternion.Euler(0f, 0f, angle);

            if (angle < 20f && angle > -40f)
            {
                playerHead.rotation = Quaternion.Euler(0f, 0f, angle);
            }

            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
    } //Facing

    public float getFire1() { return fire1; }
    
}
