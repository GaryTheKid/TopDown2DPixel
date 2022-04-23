using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] private UI_Inventory _uiInventory;
    [SerializeField] private GameObject _itemSlots;

    private PlayerStats playerStats;
    private Inventory _inventory;
    private List<EquipmentSlot> _equipmentSlots;
    private PhotonView _PV;
    private bool currLockFlag;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStatsController>().playerStats;
        _PV = GetComponent<PhotonView>();
        _inventory = new Inventory();
        _uiInventory.SetInventory(_inventory);
        _uiInventory.SpawnItemSlots();
        _equipmentSlots = _uiInventory.GetEquipmentSlots();
    }

    private void Update()
    {
        if (playerStats.isDead)
            return;

        HandleUIInventory();
        HandleUseEquipment();
    }

    private void HandleUIInventory() 
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_itemSlots.activeSelf)
            {
                _itemSlots.SetActive(false);
                playerStats.isWeaponLocked = currLockFlag;
            }
            else
            {
                _itemSlots.SetActive(true);
                currLockFlag = playerStats.isWeaponLocked;
                playerStats.isWeaponLocked = true;
            } 
        }
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