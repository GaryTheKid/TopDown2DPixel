using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NetworkCalls;

public class HealthPotion : Consumables
{
    public int healAmount;

    public HealthPotion() 
    {
        healAmount = 10;
        itemName = "Health Potion";
        itemID = 4;
        amount = 1;
        itemType = ItemType.Consumable;
    }

    public HealthPotion(short amount)
    {
        healAmount = 10;
        itemName = "HealthPotion";
        itemID = 4;
        this.amount = amount;
        itemType = ItemType.Consumable;
    }

    public override void Consume(PhotonView PV)
    {
        Debug.Log("Use health potion");
        // TODO: healing effect
        NetworkCalls.Consumables_NetWork.UseHealthPotion(PV, healAmount);

        if (amount - 1 >= 0)
        {
            amount--;
        }
        
        if (amount <= 0)
        {
            DestroySelf();
        }
    }

    public override void UseItem(PhotonView PV)
    {
        Consume(PV);
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.healthPotionSprite;
    }

    public override bool IsStackable()
    {
        return true;
    }
}
