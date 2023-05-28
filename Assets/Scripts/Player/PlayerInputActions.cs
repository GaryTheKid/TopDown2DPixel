using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputActions : MonoBehaviour
{
    public PCInputActions inputActions;

    // bind inputs
    private void Awake()
    {
        inputActions = new PCInputActions();

        // move
        inputActions.Player.Move.Enable();

        // interact
        inputActions.Player.Interact.Enable();

        // aim
        inputActions.Player.Aim.Enable();
        inputActions.Player.TouchStart.Enable();
        inputActions.Player.TouchCanceled.Enable();
        inputActions.Player.TouchPos.Enable();
        inputActions.Player.TouchDelta.Enable();

        // attack
        inputActions.Player.FireOrChargeWeapon.Enable();

        // inventory
        inputActions.Player.InventoryActivation.Enable();
        inputActions.Player.EquipmentQuickCast_1.Enable();
        inputActions.Player.EquipmentQuickCast_2.Enable();
        inputActions.Player.EquipmentQuickCast_3.Enable();
        inputActions.Player.EquipmentQuickCast_4.Enable();
        inputActions.Player.EquipmentQuickCast_5.Enable();
        inputActions.Player.EquipmentQuickCast_6.Enable();

        // respawn
        inputActions.Player.Respawn.Enable();

        // voice chat
        inputActions.Player.PushToTalk.Enable();

        // social 
        inputActions.Player.HoldSocialWheelMenu.Enable();
        inputActions.Player.ReleaseEmojiWheelMenu.Enable();
    }
}
