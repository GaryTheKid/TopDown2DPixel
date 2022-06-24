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
    [SerializeField] private Transform _playerAnchorPos;
    [SerializeField] private PhotonView _PV;
    [SerializeField] private List<Slot> _itemSlots;
    [SerializeField] private List<EquippingIndicator> _equippingIndicators;
    [SerializeField] private PlayerInventoryController _playerInventoryController;
    private Inventory _inventory;
    private int _inventorySize;
    private int _equipmentSlotCount;

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
            slotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);

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
            itemSlotRectTransform.gameObject.SetActive(true);

            // set use item logic
            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                // use item unless it is a equipable one and not be in the equipment slot
                if (i >= _equipmentSlotCount && item is IEquipable)
                {
                    return;
                }

                _inventory.UseItem(_PV, itemSlotRectTransform.GetComponent<DragDrop>().currentSlot.uiIndex);
            };

            // set drop logic
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                _playerInventoryController.DropItem(itemSlotRectTransform.GetComponent<DragDrop>().currentSlot.uiIndex);

                // if item is equipable, unequip it
                if (item is IEquipable && item.isEquipped)
                {
                    item.Unequip(_PV);
                }
            };

            // set drag drop logic
            itemSlotRectTransform.GetComponent<DragDrop>().OnChangeItemUIIndex = (int currUIIndex, int newUIIndex) => 
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
            itemSlotRectTransform.GetComponent<DragDrop>().OnDragOutSideUI = (int currUIIndex) =>
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
            itemSlotRectTransform.GetComponent<DragDrop>().OnUsingUI = () =>
            {
                // swap item pos
                _playerInventoryController.IsUsingUI = true;
            };

            // set drag drop logic
            itemSlotRectTransform.GetComponent<DragDrop>().OnFinishUsingUI = () =>
            {
                // swap item pos
                _playerInventoryController.IsUsingUI = false;
            };

            // set item ui image
            Image image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = item.GetSprite();

            // set item ui amount text
            Text uiText = itemSlotRectTransform.Find("amountText").GetComponent<Text>();
            switch (item.itemType)
            {
                default:
                    if (item.amount > 1)
                    {
                        uiText.text = item.amount.ToString();
                    }
                    else
                    {
                        uiText.text = "";
                    }
                    break;
                case Item.ItemType.Consumable:
                case Item.ItemType.Material:
                    uiText.text = item.amount.ToString(); break;
            }

            // nest the item in the slot
            itemSlotRectTransform.GetComponent<DragDrop>().currentSlot = _itemSlots[i];
            itemSlotRectTransform.anchoredPosition = _itemSlots[i].transform.GetComponent<RectTransform>().anchoredPosition;
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
