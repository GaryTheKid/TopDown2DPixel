using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
            var item = itemWorld.GetItem();
            NetworkCalls.Character.PickItem(_PV, item.itemID, itemWorld.itemWorldID, item.amount, item.durability);
        }
    }
}
