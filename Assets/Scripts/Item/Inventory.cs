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
        this.maxCapacity = 24;
        itemList = new List<Item>();
        for (int i = 0; i < maxCapacity; i++)
        {
            itemList.Add(null);
        }
    }

    public Inventory(int capacity)
    {
        // the minimal capacity is 6
        if (capacity >= 6)
            this.maxCapacity = capacity;
        else
            this.maxCapacity = 6;

        itemList = new List<Item>();
        for (int i = 0; i < maxCapacity; i++)
        {
            itemList.Add(null);
        }
    }

    public void AddItem(Item item)
    {
        // add to the list
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem == null)
                    continue;

                if (inventoryItem.itemName == item.itemName)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory)
            {
                Add(item);
            }
        }
        else
        {
            Add(item);
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
                if (inventoryItem == null)
                    continue;

                if (inventoryItem.itemName == item.itemName)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                Remove(itemInInventory);
            }
        }
        else
        {
            Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveAllItems()
    {
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

    public Item GetItemFromList(int itemIndex)
    {
        return itemList[itemIndex];
    }

    public void SwapItems(int first, int second)
    {
        Item buffer = itemList[first];
        itemList[first] = itemList[second];
        itemList[second] = buffer;
    }

    private void Add(Item item)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == null)
            {
                itemList[i] = item;
                break;
            }
        }
    }

    private void Remove(Item item)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == item)
            {
                itemList[i] = null;
                break;
            }
        }
    }
}
