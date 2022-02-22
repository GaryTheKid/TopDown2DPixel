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
    private Inventory _inventory;
    private List<ItemSlot> itemSlots;
    private List<EquipmentSlots> equipmentSlots;

    public void SpawnItemSlots()
    {
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 120f;
        for (int i = 0; i < _inventory.capacity; i++)
        {
            // instantiate empty slot template
            RectTransform slotRectTransform = Instantiate(_emptySlotTemplate, _emptySlotContainer).GetComponent<RectTransform>();
            slotRectTransform.gameObject.SetActive(true);

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

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e) 
    {
        UpdateInventoryItems();
    }

    private void UpdateInventoryItems()
    {
        foreach (Transform child in _itemSlotContainer)
        {
            if (child == _itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 120f;
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

            // set item ui image
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
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
                    } break;
                case Item.ItemType.Consumable:
                case Item.ItemType.Material:
                    uiText.text = item.amount.ToString(); break;
            }
            
            // new line
            x++;
            if (x > 5)
            {
                x = 0;
                y--;
            }
        }
    }
}
