using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Utilities;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private PlayerInventoryController _inventoryController;
    [SerializeField] private PhotonView _PV;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // interact with loot box
        LootBoxWorld lootBoxWorld = collision.GetComponent<LootBoxWorld>();
        if (lootBoxWorld != null)
        {
            NetworkCalls.Character.OpenLootBox(_PV, lootBoxWorld.lootBoxWorldID);
        }

        // pick up item
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            // get item
            Item item = itemWorld.GetItem();
            Item itemCopy = (Item)Common.GetObjectCopyFromInstance(item);
            itemCopy.amount = item.amount;
            itemCopy.durability = item.durability;

            // add item to the player Inventory
            _inventoryController.AddItem(itemCopy);

            NetworkCalls.Character.PickItem(_PV, itemWorld.itemWorldID);
        }
    }
}
