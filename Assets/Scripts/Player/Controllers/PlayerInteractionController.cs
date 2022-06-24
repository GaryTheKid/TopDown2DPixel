using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private WorldInteracter _worldInteracter;

    private PlayerInventoryController _playerInventoryController;

    private void Awake()
    {
        _playerInventoryController = GetComponent<PlayerInventoryController>();
    }

    // Update is called once per frame
    void Update()
    {
        // interact loot box
        if (Input.GetKeyDown(KeyCode.E) && _worldInteracter.lootBoxesInRange.Count > 0)
        {
            OpenLootBox();
        }

        // interact item world
        if (Input.GetKeyDown(KeyCode.E) && _worldInteracter.itemWorldsInRange.Count > 0)
        {
            PickItem();
        }
    }

    private void OpenLootBox()
    {
        var lootBoxes = _worldInteracter.lootBoxesInRange;
        var lastIndex = lootBoxes.Count - 1;
        lootBoxes[lastIndex].OpenLootBox();
    }

    private void PickItem()
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
