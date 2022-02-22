using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    public Bow()
    {
        this.itemName = "Bow";
        this.amount = 1;
        this.itemType = ItemType.RangedWeapon;
        this.durability = 100;
    }

    public Bow(int amount)
    {
        this.itemName = "Bow";
        this.amount = amount;
        this.itemType = ItemType.RangedWeapon;
        this.durability = 100;
    }

    public override void Attack()
    {
        Debug.Log("Bow Attacking");

        if (--this.durability <= 0)
            DestroySelf();
    }

    public override void Equip(PhotonView PV)
    {
        throw new System.NotImplementedException();
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfSword;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.bowSprite;
    }

    public override bool IsStackable()
    {
        return false;
    }
}
