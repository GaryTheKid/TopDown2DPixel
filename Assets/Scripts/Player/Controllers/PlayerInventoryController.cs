/* Last Edition: 05/22/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   The inventory controller attached to the player character, handling all actions relating to inventory.
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class PlayerInventoryController : MonoBehaviour
{
    public float inventoryCD;

    [SerializeField] private UI_Inventory _uiInventory;
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

    private PCInputActions _inputActions;

    private void Awake()
    {
        _playerStats = GetComponent<PlayerStatsController>().playerStats;
        _PV = GetComponent<PhotonView>();
        _inventory = new Inventory(6);
        _uiInventory.SetInventory(_inventory);
        _uiInventory.SpawnItemSlots();
        _equipmentSlots = _uiInventory.GetEquipmentSlots();

        // bind and enable input
        _inputActions = GetComponent<PlayerInputActions>().inputActions;
        //_inputActions.Player.InventoryActivation.performed += HandleUIInventory;
        LoadEquipmentQuickCast();
    }

    private void HandleUIInventory(InputAction.CallbackContext context)
    {
        if (_playerStats.isDead)
            return;

        if (context.performed)
        {
            if (_uiInventory.IsBagActive())
                CloseUIInventory();
            else
                OpenUIInventory();
        }
    }

    private void HandleUseEquipment_1(InputAction.CallbackContext context)
    {
        if (_playerStats.isDead)
            return;

        if (_playerStats.isInventoryLocked)
            return;

        if (context.performed && _inventory.GetItemFromList(0) != null)
        {
            _inventory.UseItem(_PV, 0);
        }
    }

    private void HandleUseEquipment_2(InputAction.CallbackContext context)
    {
        if (_playerStats.isDead)
            return;

        if (_playerStats.isInventoryLocked)
            return;

        if (context.performed && _inventory.GetItemFromList(1) != null)
        {
            _inventory.UseItem(_PV, 1);
        }
    }

    private void HandleUseEquipment_3(InputAction.CallbackContext context)
    {
        if (_playerStats.isDead)
            return;

        if (_playerStats.isInventoryLocked)
            return;

        if (context.performed && _inventory.GetItemFromList(2) != null)
        {
            _inventory.UseItem(_PV, 2);
        }
    }

    private void HandleUseEquipment_4(InputAction.CallbackContext context)
    {
        if (_playerStats.isDead)
            return;

        if (_playerStats.isInventoryLocked)
            return;

        if (context.performed && _inventory.GetItemFromList(3) != null)
        {
            _inventory.UseItem(_PV, 3);
        }
    }

    private void HandleUseEquipment_5(InputAction.CallbackContext context)
    {
        if (_playerStats.isDead)
            return;

        if (_playerStats.isInventoryLocked)
            return;

        if (context.performed && _inventory.GetItemFromList(4) != null)
        {
            _inventory.UseItem(_PV, 4);
        }
    }

    private void HandleUseEquipment_6(InputAction.CallbackContext context)
    {
        if (_playerStats.isDead)
            return;

        if (_playerStats.isInventoryLocked)
            return;

        if (context.performed && _inventory.GetItemFromList(5) != null)
        {
            _inventory.UseItem(_PV, 5);
        }
    }

    public void LoadEquipmentQuickCast()
    {
        _inputActions.Player.EquipmentQuickCast_1.performed += HandleUseEquipment_1;
        _inputActions.Player.EquipmentQuickCast_2.performed += HandleUseEquipment_2;
        _inputActions.Player.EquipmentQuickCast_3.performed += HandleUseEquipment_3;
        _inputActions.Player.EquipmentQuickCast_4.performed += HandleUseEquipment_4;
        _inputActions.Player.EquipmentQuickCast_5.performed += HandleUseEquipment_5;
        _inputActions.Player.EquipmentQuickCast_6.performed += HandleUseEquipment_6;
    }

    public void UnloadEquipmentQuickCast()
    {
        _inputActions.Player.EquipmentQuickCast_1.performed -= HandleUseEquipment_1;
        _inputActions.Player.EquipmentQuickCast_2.performed -= HandleUseEquipment_2;
        _inputActions.Player.EquipmentQuickCast_3.performed -= HandleUseEquipment_3;
        _inputActions.Player.EquipmentQuickCast_4.performed -= HandleUseEquipment_4;
        _inputActions.Player.EquipmentQuickCast_5.performed -= HandleUseEquipment_5;
        _inputActions.Player.EquipmentQuickCast_6.performed -= HandleUseEquipment_6;
    }

    public void OpenUIInventory()
    {
        _uiInventory.ShowBag();
        _currLockFlag_Weapon = _playerStats.isWeaponLocked;
        _currLockFlag_Movement = _playerStats.isMovementLocked;
        _currLockFlag_Inventory = _playerStats.isInventoryLocked;
        _playerStats.isWeaponLocked = true;
        _playerStats.isMovementLocked = true;
        _playerStats.isInventoryLocked = true;
    }

    public void CloseUIInventory()
    {
        _uiInventory.HideBag();
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

    public void UpdateItemDurability(short delta)
    {
        (int uiIndex, short newDurability) = _inventory.UpdateItemDurability(delta);
        if (uiIndex == -1)
        {
            print("Can't find equipped item!");
            return;
        }

        _uiInventory.UpdateItemDurabilityUI(uiIndex, newDurability);
    }

    public Item GetItem(int itemIndex)
    {
        return _inventory.GetItemFromList(itemIndex);
    }
}
