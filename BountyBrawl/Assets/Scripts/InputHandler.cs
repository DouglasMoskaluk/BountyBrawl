using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private PlayerBody player;

    private bool mainMenu;

    [SerializeField] private GameObject characterAmount;
    //[SerializeField] private GameObject PlayerAmount;

    public void Start()
    {
        mainMenu = true;
    }

    public void FindPlayer()
    {
       playerInput = GetComponent<PlayerInput>();
       var players = FindObjectsOfType<PlayerBody>();
       var index = playerInput.playerIndex;
       player = players.FirstOrDefault(p => p.GetPlayerIndex() == index);

    }

    private void Update()
    {
        if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            if (SceneManager.GetActiveScene().buildIndex != 5)
            {
                SceneManager.LoadScene(5);
            }
            else
            {
                FindObjectOfType<SceneLoader>().MainMenu();
            }
        }
    }

    public void Reset()
    {
        playerInput = null;
        player = null;
    }

    public void OnMove(CallbackContext context)
    {
        if (player != null)
        {
            player.SetInputVector(context.ReadValue<Vector2>());
        }
    }

    public void Facing(CallbackContext context)
    {
        if (player != null)
        {
            player.SetFacingVector(context.ReadValue<Vector2>());
        }
    }

    public void Fire1(CallbackContext context)
    {
        if (player != null)
        {
            player.Fire1(context.ReadValue<float>());
        }
    }

    public void Fire2(CallbackContext context)
    {
        if(player != null)
        {
            player.Fire2(context.ReadValue<float>());
        }
    }

    public void Throw(CallbackContext context)
    {
        if(player != null)
        {
            
            player.Throw(context.ReadValue<float>());
        }
    }

    public void Pause(CallbackContext context)
    {
        if (player != null)
        {
            player.Pause(context.performed);
        }
    }

    public void RemoveHealth(CallbackContext context)
    {
        /*
        if (player != null)
        {
            player.RemoveHealth(context.ReadValue<float>());
        }
        */
    }

    public void AddHealth(CallbackContext context)
    {
        /*
        if (player != null)
        {
            player.AddHealth(context.ReadValue<float>());
        }
        */
    }

    public void Respawn(CallbackContext context)
    {
        /*
        if (player != null)
        {
            player.Respawn(context.ReadValue<float>());
        }
        */
    }

    public void ActivateEvent(CallbackContext context)
    {
    }

    public void Square(CallbackContext context)
    {
        if (mainMenu == true) 
        {
            FindPlayer();
            if (player != null)
            {
                player.Square(context.performed);
            }
        }
    }

    public void Triangle(CallbackContext context)
    {
        
        
            FindPlayer();
            if (player != null)
            {
                player.Triangle(context.performed);
            }
        
    }

    public void Circle(CallbackContext context)
    {

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>().MainMenu();
        }

    }

    public void NotMainMenu()
    {
        mainMenu = false;
    }
    public void IsMainMenu()
    {
        mainMenu = true;
    }

}
