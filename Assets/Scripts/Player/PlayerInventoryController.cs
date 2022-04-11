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
        _uiInventory.SetInventory(_inventory);
        _uiInventory.SpawnItemSlots();
        _equipmentSlots = _uiInventory.GetEquipmentSlots();
    }

    private void Update()
    {
        HandleUseEquipment();
    }

    private void HandleUseEquipment()
    {
        foreach (EquipmentSlot slot in _equipmentSlots)
        {
            if (Input.GetKeyDown(slot.keyCode) && slot.SlotItem != null)
            {
                _inventory.UseItem(_PV, slot.SlotItem);
            }
        }
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }

    public void PickItem(Item item)
    {
        _inventory.AddItem(item);
    }
}
