using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigHealthPotion : HealthPotion
{
    public BigHealthPotion()
    {
        healAmount = 30;
        itemName = "BigHealthPotion";
        itemID = 8;
        amount = 1;
        itemType = ItemType.Consumable;
    }

    public BigHealthPotion(short amount)
    {
        healAmount = 30;
        itemName = "BigHealthPotion";
        itemID = 8;
        this.amount = amount;
        itemType = ItemType.Consumable;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.bigHealthPotionSprite;
    }
}
