using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperHealthPotion : HealthPotion
{
    public SuperHealthPotion()
    {
        healAmount = 50;
        itemName = "Super Health Potion";
        itemID = 12;
        amount = 1;
        itemType = ItemType.Consumable;
    }

    public SuperHealthPotion(short amount)
    {
        healAmount = 50;
        itemName = "Super Health Potion";
        itemID = 12;
        this.amount = amount;
        itemType = ItemType.Consumable;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.superHealthPotionSprite;
    }
}
