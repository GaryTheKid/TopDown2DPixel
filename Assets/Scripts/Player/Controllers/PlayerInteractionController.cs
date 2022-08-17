/* Last Edition: 06/28/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The interaction controller for the player to interact with interactable world objs.
 * Last Edition:
 *   Just Created.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private WorldInteracter _worldInteracter;

    private PlayerInventoryController _playerInventoryController;

    private PCInputActions _inputActions;

    private void Awake()
    {
        _playerInventoryController = GetComponent<PlayerInventoryController>();

        _inputActions = GetComponent<PlayerInputActions>().inputActions;
        _inputActions.Player.Interact.performed += OpenLootBox;
        _inputActions.Player.Interact.performed += PickItem;
        _inputActions.Player.Interact.performed += InteractWithWell;
    }

    private void OpenLootBox(InputAction.CallbackContext context)
    {
        if (context.performed && _worldInteracter.lootBoxesInRange.Count > 0)
        {
            var lootBoxes = _worldInteracter.lootBoxesInRange;
            var lastIndex = lootBoxes.Count - 1;
            lootBoxes[lastIndex].OpenLootBox();
        }
    }

    private void PickItem(InputAction.CallbackContext context)
    {
        if (context.performed && _worldInteracter.itemWorldsInRange.Count > 0)
        {
            var itemWorlds = _worldInteracter.itemWorldsInRange;
            var lastIndex = itemWorlds.Count - 1;

            // get item
            Item item = itemWorlds[lastIndex].item;
            Item itemCopy = (Item)Common.GetObjectCopyFromInstance(item);
            itemCopy.amount = item.amount;
            itemCopy.durability = item.durability;

            // check if inventory is full
            bool isInventoryFull = _playerInventoryController.AddItem(itemCopy);
            if (!isInventoryFull)
            {
                // destroy the picked item
                itemWorlds[lastIndex].PickItem();
            }
            else
            {
                // TODO: display inventory full effect
                print("Inventory is full");
            }
        }
    }

    private void InteractWithWell(InputAction.CallbackContext context)
    {
        if (context.performed && _worldInteracter.wellInRange != null)
        {
            // drink
            _worldInteracter.wellInRange.Drink();
        }
    }
}
