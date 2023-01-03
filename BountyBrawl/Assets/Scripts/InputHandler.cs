using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class InputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerBody player;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        var players = FindObjectsOfType<PlayerBody>();
        var index = playerInput.playerIndex;
        player = players.FirstOrDefault(p => p.GetPlayerIndex() == index);
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

}
