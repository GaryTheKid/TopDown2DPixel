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
    [SerializeField] private Transform _emptySlotContainer;
    [SerializeField] private Transform _emptySlotTemplate;
    [SerializeField] private Transform _itemSlotContainer;
    [SerializeField] private Transform _itemSlotTemplate;
    [SerializeField] private Transform _playerAnchorPos;
    [SerializeField] private PhotonView _PV;
    [SerializeField] private List<EquipmentSlot> _equipmentSlots;
    private List<ItemSlot> _emptySlots;
    private Inventory _inventory;

    public void SpawnItemSlots()
    {
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 120f;

        for (int i = 0; i < _inventory.maxCapacity - 6; i++)
        {
            // instantiate empty slot template
            RectTransform slotRectTransform = Instantiate(_emptySlotTemplate, _emptySlotContainer).GetComponent<RectTransform>();
            slotRectTransform.gameObject.SetActive(true);

            // update empty slot list
            ItemSlot itemSlot = slotRectTransform.GetComponent<ItemSlot>();
            _emptySlots.Add(itemSlot);
            itemSlot.uiIndex = i + 6;

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

        for (int i = 0; i < _equipmentSlots.Count; i++)
        {
            // update empty slot list
            _equipmentSlots[i].uiIndex = i;
        }
    }

    public void SetInventory(Inventory inventory)
    {
        this._inventory = inventory;
        _emptySlots = new List<ItemSlot>();

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        UpdateInventoryItems();
    }

    public List<EquipmentSlot> GetEquipmentSlots() 
    {
        return _equipmentSlots;
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e) 
    {
        UpdateInventoryItems();
    }

    private void UpdateInventoryItems()
    {
        // clear ui
        foreach (Transform child in _itemSlotContainer)
        {
            if (child == _itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        // create ui
        for (int i = 0; i < _inventory.GetItemList().Count; i++)
        {
            // get item from list
            Item item = _inventory.GetItemFromList(i);
            if (item == null)
                continue;

            // instantiate item template
            RectTransform itemSlotRectTransform = Instantiate(_itemSlotTemplate, _itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            // set use item logic
            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                _inventory.UseItem(_PV, item);
            };

            // set drop logic
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                Item itemCopy = (Item)Common.GetObjectCopyFromInstance(item);
                itemCopy.amount = item.amount;
                _inventory.RemoveItem(item);
                ItemWorld.DropItem(_playerAnchorPos.position, itemCopy);

                // if item is equipable, unequip it
                if (item is IEquipable)
                {
                    item.Unequip(_PV);
                }
            };

            // set drag drop logic
            itemSlotRectTransform.GetComponent<DragDrop>().OnChangeItemUIIndex = (int currUIIndex, int newUIIndex) => 
            {
                // swap item pos
                _inventory.SwapItems(currUIIndex, newUIIndex);

                // if item is equipable, drag it from equipment slots will unequip it
                if (item is IEquipable)
                {
                    item.Unequip(_PV);
                }
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
            if (i > 6)
            {
                itemSlotRectTransform.GetComponent<DragDrop>().currentSlot = _emptySlots[i - 6];
                itemSlotRectTransform.anchoredPosition = _emptySlots[i - 6].transform.GetComponent<RectTransform>().anchoredPosition;
            }
            else 
            {
                itemSlotRectTransform.GetComponent<DragDrop>().currentSlot = _equipmentSlots[i];
                itemSlotRectTransform.anchoredPosition = _equipmentSlots[i].transform.GetComponent<RectTransform>().anchoredPosition;
            }
        }
    }
}
