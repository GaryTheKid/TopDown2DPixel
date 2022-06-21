using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpeedPotion : Consumables
{
    public float boostAmount;
    public float effectTime;

    public SpeedPotion()
    {
        boostAmount = 20f;
        effectTime = 6f;
        this.itemName = "SpeedPotion";
        itemID = 7;
        this.amount = 1;
        this.itemType = ItemType.Consumable;
    }

    public SpeedPotion(short amount)
    {
        boostAmount = 20f;
        effectTime = 6f;
        this.itemName = "SpeedPotion";
        itemID = 7;
        this.amount = amount;
        this.itemType = ItemType.Consumable;
    }

    public override void Consume(PhotonView PV)
    {
        Debug.Log("Use speed potion, " + "boost: 20, time: 6s");
        // TODO: healing effect
        NetworkCalls.Consumables_NetWork.UseSpeedPotion(PV, boostAmount, effectTime);

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
        return ItemAssets.itemAssets.speedPotionSprite;
    }

    public override bool IsStackable()
    {
        return true;
    }
}
