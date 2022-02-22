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
        this.itemName = "HealthPotion";
        this.amount = 1;
        this.itemType = ItemType.Consumable;
    }

    public HealthPotion(int amount)
    {
        this.itemName = "HealthPotion";
        this.amount = amount;
        this.itemType = ItemType.Consumable;
    }

    public override void Consume(PhotonView PV)
    {
        Debug.Log("Use health potion");
        // TODO: healing effect
        NetworkCalls.Consumables.UseHealthPotion(PV);

        if (--amount <= 0)
            DestroySelf();
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
