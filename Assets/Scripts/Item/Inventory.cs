using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Inventory
{
    public int maxCapacity;
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;

    public Inventory()
    {
        this.maxCapacity = 18;
        itemList = new List<Item>();
    }

    public Inventory(int capacity)
    {
        this.maxCapacity = capacity;
        itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        // check if inventory is full
        if (itemList.Count >= maxCapacity)
        {
            Debug.Log("Inventory is full!");
            return;
        }

        // add to the list
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemName == item.itemName)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }

        // bind destroyself action
        item.destroySelfAction = () => 
        {
            if (item.amount <= 0)
            {
                RemoveItem(item);
            }
        };

        // update list ui
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {       
                if (inventoryItem.itemName == item.itemName)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            itemList.Remove(item);
        }
        item.uiIndex = 0;
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveAllItems()
    {
        foreach (Item item in itemList)
        {
            item.uiIndex = 0;
        }
        itemList.Clear();
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(PhotonView PV, Item item)
    {
        item.UseItem(PV);
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
