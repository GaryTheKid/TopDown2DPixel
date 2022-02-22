using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] private UI_Inventory uiInventory;

    private Inventory inventory;
    private PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        inventory = new Inventory();
    }

    private void Start()
    {
        uiInventory.SetInventory(inventory);
        uiInventory.SpawnItemSlots();
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // pick up item
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    public Inventory GetInventory()
    {
        return inventory;
    }
}
