/* Last Edition: 05/22/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The inventory controller attached to the player character, handling all actions relating to inventory.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInventoryController : MonoBehaviour
{
    public float inventoryCD;

    [SerializeField] private UI_Inventory _uiInventory;
    [SerializeField] private GameObject _itemSlots;
    [SerializeField] private Transform _playerAnchorPos;

    private PlayerStats _playerStats;
    private Inventory _inventory;
    private List<Slot> _equipmentSlots;
    private PhotonView _PV;
    private bool _currLockFlag_Movement;
    private bool _currLockFlag_Weapon;
    private bool _currLockFlag_Inventory;
    private bool _isUsingUI;
    public bool IsUsingUI 
    {
        get { return _isUsingUI; }
        set { _isUsingUI = value; }
    }

    private void Awake()
    {
        _playerStats = GetComponent<PlayerStatsController>().playerStats;
        _PV = GetComponent<PhotonView>();
        _inventory = new Inventory();
        _uiInventory.SetInventory(_inventory);
        _uiInventory.SpawnItemSlots();
        _equipmentSlots = _uiInventory.GetEquipmentSlots();
    }

    private void Update()
    {
        if (_playerStats.isDead)
            return;

        HandleUIInventory();
        HandleUseEquipment();
    }

    private void HandleUIInventory() 
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (_itemSlots.activeSelf)
                CloseUIInventory();
            else
                OpenUIInventory();
        }
    }

    private void HandleUseEquipment()
    {
        if (_playerStats.isInventoryLocked)
            return;

        for (int i = 0; i < _equipmentSlots.Count; i++)
        {
            if (Input.GetKeyDown(_equipmentSlots[i].keyCode) && _inventory.GetItemFromList(i) != null)
            {
                _inventory.UseItem(_PV, i);
            }
        }
    }

    public void OpenUIInventory()
    {
        _itemSlots.SetActive(true);
        _currLockFlag_Weapon = _playerStats.isWeaponLocked;
        _currLockFlag_Movement = _playerStats.isMovementLocked;
        _currLockFlag_Inventory = _playerStats.isInventoryLocked;
        _playerStats.isWeaponLocked = true;
        _playerStats.isMovementLocked = true;
        _playerStats.isInventoryLocked = true;
    }

    public void CloseUIInventory()
    {
        _itemSlots.SetActive(false);
        _playerStats.isWeaponLocked = _currLockFlag_Weapon;
        _playerStats.isMovementLocked = _currLockFlag_Movement;
        _playerStats.isInventoryLocked = _currLockFlag_Inventory;
    }

    public void LockInventory()
    {
        _playerStats.isInventoryLocked = true;
    }

    public void UnlockInventory()
    {
        _playerStats.isInventoryLocked = false;
    }

    public bool IsInventoryLocked()
    {
        return _playerStats.isInventoryLocked;
    }

    public void SetInventoryOnCD(float cd)
    {
        _uiInventory.SetInventoryOnCD(cd);
    }

    public Vector2 GetAnchorPos()
    {
        return _playerAnchorPos.position;
    }

    public void DiscardAllItems()
    {
        var itemList = _inventory.GetItemList();
        for (short i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] != null)
            {
                DropItem(i);
            }
        }
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }

    public bool AddItem(Item item)
    {
        return _inventory.AddItem(item);
    }

    public void DropItem(short itemIndex)
    {
        var item = GetItem(itemIndex);
        if (item != null)
        {
            NetworkCalls.Player_NetWork.DropItem(_PV, GetAnchorPos(), item.itemID, item.amount, item.durability);
        }

        // remove item from inventory
        _inventory.RemoveItem(item);
    }

    public Item GetItem(int itemIndex)
    {
        return _inventory.GetItemFromList(itemIndex);
    }
}
