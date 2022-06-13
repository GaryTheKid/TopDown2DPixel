using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InvinciblePotion : Consumables
{
    public float effectTime;

    public InvinciblePotion()
    {
        effectTime = 2f;
        itemName = "InvinciblePotion";
        itemID = 9;
        amount = 1;
        itemType = ItemType.Consumable;
    }

    public InvinciblePotion(short amount)
    {
        effectTime = 2f;
        itemName = "InvinciblePotion";
        itemID = 9;
        this.amount = amount;
        itemType = ItemType.Consumable;
    }

    public override void Consume(PhotonView PV)
    {
        Debug.Log("Use invincible potion, " + "time: 2s");
        // TODO: healing effect
        NetworkCalls.Consumables.UseInvinciblePotion(PV, effectTime);

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
        return ItemAssets.itemAssets.invinciblePotionSprite;
    }

    public override bool IsStackable()
    {
        return true;
    }
}
