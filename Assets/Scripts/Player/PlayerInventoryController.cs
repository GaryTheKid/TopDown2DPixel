using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] private UI_Inventory _uiInventory;

    private Inventory _inventory;
    private List<EquipmentSlot> _equipmentSlots;
    private PhotonView _PV;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _inventory = new Inventory();
    }

    private void Start()
    {
        _uiInventory.SetInventory(_inventory);
        _uiInventory.SpawnItemSlots();
        _equipmentSlots = _uiInventory.GetEquipmentSlots();
    }

    private void Update()
    {
        HandleUseEquipment();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // pick up item
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            _inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
    }

    private void HandleUseEquipment()
    {
        foreach (EquipmentSlot slot in _equipmentSlots)
        {
            if (Input.GetKeyDown(slot.keyCode))
            {
                if(slot.SlotItem != null)
                    slot.SlotItem.UseItem(_PV);
            }
        }
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }
}
