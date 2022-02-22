using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlots
{
    public event EventHandler OnEquipmentListChanged;
    private List<Item> EquipmentList;

    public EquipmentSlots()
    {
        EquipmentList = new List<Item>(6);
    }

    public void EquipItem(Item item, int index)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyEquipment = false;
            foreach (Item equipmentItem in EquipmentList)
            {
                if (equipmentItem.itemName == item.itemName)
                {
                    equipmentItem.amount += item.amount;
                    itemAlreadyEquipment = true;
                }
            }
            if (!itemAlreadyEquipment)
            {
                EquipmentList.Add(item);
            }
        }
        else
        {
            EquipmentList.Add(item);
        }
        OnEquipmentListChanged?.Invoke(this, EventArgs.Empty);
    }

    public List<Item> GetEquipmentList()
    {
        return EquipmentList;
    }
}
