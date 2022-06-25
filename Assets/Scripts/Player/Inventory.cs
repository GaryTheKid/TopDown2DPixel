/* Last Edition: 05/22/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   Inventory system: first 6(adjustable) slots are the equipment slots, the remaining ones
 *   are bag slots.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Inventory
{
    public int maxCapacity;
    public event EventHandler OnItemListChanged;
    public event EventHandler OnInventoryCD;
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

    public bool IsInventoryFull()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == null)
            {
                Debug.Log(i + " is empty");
                return false;
            }
        }

        return true;
    }

    public bool AddItem(Item item)
    {
        bool isInventoryFull = false;

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
                isInventoryFull = Add(item);
            }
        }
        else
        {
            isInventoryFull = Add(item);
        }

        // check if inventory is full
        if (isInventoryFull)
            return isInventoryFull;

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

        return isInventoryFull;
    }

    public void RemoveOneItem(Item item)
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
                    inventoryItem.amount -= 1;
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

    public void RemoveItemAt(int index)
    {
        Item item = itemList[index];
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
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i] = null;
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(PhotonView PV, int index)
    {
        if (itemList[index] == null) 
        {
            Debug.Log("Item is null");
            return;
        }

        itemList[index].UseItem(PV);

        // set all the other items unequipped
        for (int i = 0; i < itemList.Count; i++)
        {
            if (!(itemList[index] is IEquipable))
                break;

            if (itemList[i] == null || i == index)
                continue;

            if(itemList[i] is IEquipable)
                itemList[i].isEquipped = false;
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        OnInventoryCD?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public short GetItemAmountListById(short id)
    {
        short amount = 0;

        foreach (var itemInInventory in itemList)
        {
            if (itemInInventory == null)
                continue;

            if (id == itemInInventory.itemID)
            {
                amount += itemInInventory.amount;
            }
        }

        return amount;
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

    private bool Add(Item item)
    {
        if (item.amount <= 0)
            return true;

        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == null)
            {
                itemList[i] = item;
                return false;
            }
        }

        DebugGUI.debugGUI.ShowDebugTag("Inventory is full!", 5f);
        return true;
    }

    private void Remove(Item item)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] == item)
            {
                itemList[i] = null;
                return;
            }
        }
    }

#if DEBUG
    public void Debug_PrintItemCount()
    {
        int count = 0;
        string allItems = "";
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i] != null)
            {
                count++;
                allItems += i.ToString() + ":" + itemList[i].itemName + " ";
            }
        }

        Debug.Log(allItems);
    }
#endif
}
