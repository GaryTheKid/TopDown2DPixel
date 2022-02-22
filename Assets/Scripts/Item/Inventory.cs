using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Inventory
{
    public int capacity;
    public event EventHandler OnItemListChanged;
    private List<Item> itemList;

    public Inventory()
    {
        this.capacity = 18;
        itemList = new List<Item>();
    }

    public Inventory(int capacity)
    {
        this.capacity = capacity;
        itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
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
