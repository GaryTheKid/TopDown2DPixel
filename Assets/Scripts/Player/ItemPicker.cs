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
        // pick up item
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            var item = itemWorld.GetItem();
            NetworkCalls.Character.PickItem(_PV, item.itemID, itemWorld.itemWorldID, item.amount, item.durability);
        }
    }
}
