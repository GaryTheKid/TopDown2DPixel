using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using NetworkCalls;

public class Sword : Weapon
{
    public Sword()
    {
        this.itemName = "Sword";
        this.amount = 1;
        this.itemType = ItemType.MeleeWeapon;
        this.durability = 100;
    }

    public Sword(int amount)
    {
        this.itemName = "Sword";
        this.amount = amount;
        this.itemType = ItemType.MeleeWeapon;
        this.durability = 100;
    }

    public override void Attack()
    {
        Debug.Log("Sword Attacking");

        if (--this.durability <= 0)
            DestroySelf();
    }

    public override void Equip(PhotonView PV)
    {
        NetworkCalls.Weapon.EquipSword(PV);
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfSword;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.swordSprite;
    }

    public override bool IsStackable()
    {
        return false;
    }
}
