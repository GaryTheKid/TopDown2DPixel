/* Last Edition: 06/25/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   Inventory UI, manipulating all ui elements.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using Utilities;
using Photon.Pun;

public class UI_Inventory : MonoBehaviour
{
    [SerializeField] private Transform _equipmentSlotContainer;
    [SerializeField] private Transform _emptySlotContainer;
    [SerializeField] private Transform _emptySlotTemplate;
    [SerializeField] private Transform _itemContainer;
    [SerializeField] private Transform _itemTemplate;
    [SerializeField] private GameObject _bagSlotContainer; 
    [SerializeField] private Transform _playerAnchorPos;
    [SerializeField] private PhotonView _PV;
    [SerializeField] private List<Slot> _itemSlots;
    [SerializeField] private List<EquippingIndicator> _equippingIndicators;
    [SerializeField] private PlayerInventoryController _playerInventoryController;
    [SerializeField] private PlayerWeaponController _playerWeaponController;
    [SerializeField] private Image _inventoryCDImage;
    private Inventory _inventory;
    private int _inventorySize;
    private int _equipmentSlotCount;

    private IEnumerator _inventoryCD_Co;

    public void SpawnItemSlots()
    {
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 120f;
        _inventorySize = _inventory.maxCapacity;

        _equipmentSlotCount = _equipmentSlotContainer.childCount / 2;
        for (short i = 0; i < _equipmentSlotCount; i++)
        {
            // equipment slots
            Slot equipmentSlot = _equipmentSlotContainer.GetChild(i).GetComponent<Slot>();
            equipmentSlot.uiIndex = i;
            _itemSlots.Add(equipmentSlot);

            // equiping indicators
            _equippingIndicators.Add(_equipmentSlotContainer.GetChild(i + _equipmentSlotCount).GetComponent<EquippingIndicator>());
        }

        for (int i = 0; i < _inventory.maxCapacity - _equipmentSlotCount; i++)
        {
            // instantiate empty slot template
            RectTransform slotRectTransform = Instantiate(_emptySlotTemplate, _emptySlotContainer).GetComponent<RectTransform>();
            slotRectTransform.gameObject.SetActive(true);

            // update empty slot list
            Slot itemSlot = slotRectTransform.GetComponent<Slot>();
            itemSlot.uiIndex = (short)(i + _equipmentSlotCount);
            _itemSlots.Add(itemSlot);

            // set position
            Vector2 initPos = _emptySlotTemplate.GetComponent<RectTransform>().anchoredPosition;
            slotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize) + initPos;

            // new line
            x++;
            if (x > 5)
            {
                x = 0;
                y--;
            }
        }
    }

    public void SetInventory(Inventory inventory)
    {
        this._inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        inventory.OnInventoryCD += Inventory_OnInventoryCD;
        inventory.OnUnequipItem += Inventory_OnUnequipItem;
        UpdateInventoryItems();
    }

    public List<Slot> GetEquipmentSlots() 
    {
        return _itemSlots;
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e) 
    {
        UpdateInventoryItems();
    }

    private void Inventory_OnInventoryCD(object sender, System.EventArgs e)
    {
        SetInventoryOnCD(_playerInventoryController.inventoryCD);
    }

    private void Inventory_OnUnequipItem(object sender, System.EventArgs e)
    {
        NetworkCalls.Weapon_Network.UnequipWeapon(GetComponentInParent<PhotonView>());
    }

    public void UpdateInventoryItems()
    {
        // clear ui
        foreach (Transform child in _itemContainer)
        {
            if (child == _itemTemplate) continue;
            Destroy(child.gameObject);
        }

        // clear indicators
        ClearEquippingIndicator();

        // create ui
        for (int i = 0; i < _inventorySize; i++)
        {
            // get item from list
            Item item = _inventory.GetItemFromList(i);
            if (item == null)
                continue;
            if (item.amount <= 0)
            {
                item = null;
                continue;
            }

            // show equipping indicator;
            if (item.isEquipped)
                ShowEquippingIndicator(i);

            // instantiate item template
            RectTransform itemSlotRectTransform = Instantiate(_itemTemplate, _itemContainer).GetComponent<RectTransform>();
            if (i < _equipmentSlotCount || _bagSlotContainer.activeInHierarchy)
                itemSlotRectTransform.gameObject.SetActive(true);

            // set use item logic
            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                // check if inventory is locked
                if (_playerInventoryController.IsInventoryLocked())
                    return;

                _inventory.UseItem(_PV, itemSlotRectTransform.GetComponent<UI_Item>().currentSlot.uiIndex);
            };

            // set drop logic
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                // check if inventory is on CD
                if (_playerInventoryController.IsInventoryLocked())
                    return;

                _playerInventoryController.DropItem(itemSlotRectTransform.GetComponent<UI_Item>().currentSlot.uiIndex);

                // if item is equipable, unequip it
                if (item is IEquipable && item.isEquipped)
                {
                    item.Unequip(_PV);
                }
            };

            // set drag drop logic
            itemSlotRectTransform.GetComponent<UI_Item>().OnChangeItemUIIndex = (int currUIIndex, int newUIIndex) => 
            {
                // swap item in inventory
                _inventory.SwapItems(currUIIndex, newUIIndex);

                // if item is equipable, drag it from equipment slots will unequip it
                if (item is IEquipable && newUIIndex >= _equipmentSlotCount && item.isEquipped)
                {
                    item.Unequip(_PV);
                }

                // update list
                UpdateInventoryItems();
            };

            // set drag drop logic
            itemSlotRectTransform.GetComponent<UI_Item>().OnDragOutSideUI = (int currUIIndex) =>
            {
                // swap item pos
                _playerInventoryController.DropItem((short)currUIIndex);

                // if item is equipable, unequip it
                if (item is IEquipable && item.isEquipped)
                {
                    item.Unequip(_PV);
                }
            };

            // set drag drop logic
            itemSlotRectTransform.GetComponent<UI_Item>().OnUsingUI = () =>
            {
                // swap item pos
                _playerInventoryController.IsUsingUI = true;
            };

            // set drag drop logic
            itemSlotRectTransform.GetComponent<UI_Item>().OnFinishUsingUI = () =>
            {
                // swap item pos
                _playerInventoryController.IsUsingUI = false;
            };

            // set item ui image
            itemSlotRectTransform.Find("image").GetComponent<Image>().sprite = item.GetSprite();

            // set durability icon
            itemSlotRectTransform.Find("durabilityIcon").GetComponent<Image>().sprite = item.GetDurabilitySprite();
            
            // set item ui amount text and durability
            Text uiAmountText = itemSlotRectTransform.Find("amountText").GetComponent<Text>();
            Text uiDurabilityText = itemSlotRectTransform.Find("durabilityText").GetComponent<Text>();
            switch (item.itemType)
            {
                case Item.ItemType.Consumable:
                case Item.ItemType.Material:
                case Item.ItemType.ThrowableWeapon:
                case Item.ItemType.DeployableWeapon:
                    uiAmountText.text = item.amount.ToString(); 
                    break;

                default:
                    if (item.amount > 1)
                    {
                        uiAmountText.text = item.amount.ToString();
                    }
                    else
                    {
                        uiAmountText.text = "";
                    }
                    uiDurabilityText.text = item.durability.ToString();
                    break;
            }

            // nest the item in the slot
            itemSlotRectTransform.GetComponent<UI_Item>().currentSlot = _itemSlots[i];
            itemSlotRectTransform.anchoredPosition = _itemSlots[i].transform.GetComponent<RectTransform>().anchoredPosition;
        }
    }

    public void UpdateItemDurabilityUI(int index, short newDurability)
    {
        foreach (UI_Item ui_item in _itemContainer.GetComponentsInChildren<UI_Item>())
        {
            if(ui_item.currentSlot.uiIndex == index)
            {
                ui_item.transform.Find("durabilityText").GetComponent<Text>().text = newDurability.ToString();
                return;
            }
        }
    }

    public void SetInventoryOnCD(float cd)
    {
        if (_inventoryCD_Co != null)
        {
            StopCoroutine(_inventoryCD_Co);
            _inventoryCD_Co = null;
        }

        _inventoryCD_Co = Co_inventoryCD(cd);
        StartCoroutine(_inventoryCD_Co);
    }
    IEnumerator Co_inventoryCD(float cd)
    {
        // lock
        _playerInventoryController.LockInventory();

        // show cd ui
        var time = 0f;
        while (time < cd)
        {
            _inventoryCDImage.fillAmount = 1f - (time / cd);

            // wait CD update
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        // restore cd ui
        _inventoryCDImage.fillAmount = 0f;

        // unlock
        _playerInventoryController.UnlockInventory();
    }

    public bool IsBagActive()
    {
        return _bagSlotContainer.activeSelf;
    }

    public void ShowBag()
    {
        _bagSlotContainer.SetActive(true);
        foreach (Transform ui_Item in _itemContainer)
        {
            if (ui_Item == _itemTemplate)
                continue;

            ui_Item.gameObject.SetActive(true);
        }
    }

    public void HideBag()
    {
        _bagSlotContainer.SetActive(false);
        foreach (Transform ui_Item in _itemContainer)
        {
            if (ui_Item == _itemTemplate)
                continue;

            if (ui_Item.GetComponent<UI_Item>().currentSlot.uiIndex >= _equipmentSlotCount)
                ui_Item.gameObject.SetActive(false);
        }
    }

    private void ShowEquippingIndicator(int index)
    {
        for (int i = 0; i < _equipmentSlotCount; i++)
        {
            if (i == index)
            {
                _equippingIndicators[i].gameObject.SetActive(true);
            }
        }
    }

    private void ClearEquippingIndicator()
    {
        for (int i = 0; i < _equipmentSlotCount; i++)
        {
            _equippingIndicators[i].gameObject.SetActive(false);
        }
    }
}
