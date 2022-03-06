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
    public List<ItemSlot> _emptySlots;
    private Inventory _inventory;

    public void SpawnItemSlots()
    {
        _emptySlots = new List<ItemSlot>();

        int x = 0;
        int y = 0;
        float itemSlotCellSize = 120f;

        for (int i = 0; i < _inventory.maxCapacity; i++)
        {
            // instantiate empty slot template
            RectTransform slotRectTransform = Instantiate(_emptySlotTemplate, _emptySlotContainer).GetComponent<RectTransform>();
            slotRectTransform.gameObject.SetActive(true);

            // update empty slot list
            ItemSlot itemSlot = slotRectTransform.GetComponent<ItemSlot>();
            _emptySlots.Add(itemSlot);
            itemSlot.uiIndex = i + 1;

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
            _equipmentSlots[i].uiIndex = -i - 1;
        }
    }

    public void SetInventory(Inventory inventory)
    {
        this._inventory = inventory;

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
        foreach (Item item in _inventory.GetItemList())
        {
            // instantiate item template
            RectTransform itemSlotRectTransform = Instantiate(_itemSlotTemplate, _itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            // set use item logic
            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                _inventory.UseItem(_PV, item);
            };

            // set right click logic
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                Item itemCopy = (Item)Common.GetObjectCopyFromInstance(item);
                itemCopy.amount = item.amount;
                _inventory.RemoveItem(item);
                ItemWorld.DropItem(_playerAnchorPos.position, itemCopy);
            };

            // set drag drop logic
            itemSlotRectTransform.GetComponent<DragDrop>().OnChangeItemUIIndex = (int newUIIndex) => 
            {
                item.uiIndex = newUIIndex;
                itemSlotRectTransform.GetComponent<DragDrop>().currentSlot.SlotItem = item;
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

            // set item item position
            if (item.uiIndex == 0)
            {
                // nest the item in the first empty slot
                for (int i = 0; i < _emptySlots.Count; i++)
                {
                    if (_emptySlots[i].SlotItem == null)
                    {
                        item.uiIndex = i + 1;
                        _emptySlots[i].SlotItem = item;
                        itemSlotRectTransform.GetComponent<DragDrop>().currentSlot = _emptySlots[i];
                        itemSlotRectTransform.anchoredPosition = _emptySlots[i].transform.GetComponent<RectTransform>().anchoredPosition;
                        break;
                    }
                }
            }
            // nest the item in its slot position
            else if (item.uiIndex > 0)
            {
                // if located in inventory
                _emptySlots[item.uiIndex - 1].SlotItem = item;
                itemSlotRectTransform.GetComponent<DragDrop>().currentSlot = _emptySlots[item.uiIndex - 1];
                itemSlotRectTransform.anchoredPosition = _emptySlots[item.uiIndex - 1].transform.GetComponent<RectTransform>().anchoredPosition;
            }
            else
            {
                // if located in equipment slots
                _equipmentSlots[-item.uiIndex - 1].SlotItem = item;
                itemSlotRectTransform.GetComponent<DragDrop>().currentSlot = _emptySlots[-item.uiIndex - 1];
                itemSlotRectTransform.anchoredPosition = _equipmentSlots[-item.uiIndex - 1].transform.GetComponent<RectTransform>().anchoredPosition;
            }
        }
    }
}
