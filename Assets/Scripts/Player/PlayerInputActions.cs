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

        //

        // attack
        inputActions.Player.MouseClick.Enable();
        inputActions.Player.MouseHold.Enable();

        // inventory
        inputActions.Player.InventoryActivation.Enable();
        inputActions.Player.EquipmentQuickCast_1.Enable();
        inputActions.Player.EquipmentQuickCast_2.Enable();
        inputActions.Player.EquipmentQuickCast_3.Enable();
        inputActions.Player.EquipmentQuickCast_4.Enable();
        inputActions.Player.EquipmentQuickCast_5.Enable();
        inputActions.Player.EquipmentQuickCast_6.Enable();
    }
}
