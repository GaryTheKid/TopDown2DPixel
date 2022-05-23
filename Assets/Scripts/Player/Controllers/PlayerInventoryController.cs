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
    [SerializeField] private UI_Inventory _uiInventory;
    [SerializeField] private GameObject _itemSlots;
    [SerializeField] private Transform _playerAnchorPos;

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
        for (int i = 0; i < _equipmentSlots.Count; i++)
        {
            if (Input.GetKeyDown(_equipmentSlots[i].keyCode) && _inventory.GetItemFromList(i) != null)
            {
                _inventory.UseItem(_PV, i);
            }
        }
    }

    public void DiscardAllItems()
    {
        foreach (Item item in _inventory.GetItemList())
        {
            ItemWorld.DropItem(_playerAnchorPos.position, item);
        }
        _inventory.RemoveAllItems();
    }

    public Inventory GetInventory()
    {
        return _inventory;
    }

    public void PickItem(Item item)
    {
        _inventory.AddItem(item);
    }

    public void DropItem(int itemIndex)
    {
        
    }

    public Item GetItem(int itemIndex)
    {
        return _inventory.GetItemFromList(itemIndex);
    }
}
