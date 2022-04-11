using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] private PlayerInventoryController _inventoryController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // pick up item
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            _inventoryController.PickItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }
}
