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
        this.attackDmg = 15f;
        this.attackRange = 10f;
        this.attackSpeed = 1f;
    }

    public Sword(int amount)
    {
        this.itemName = "Sword";
        this.amount = amount;
        this.itemType = ItemType.MeleeWeapon;
        this.attackDmg = 15;
        this.attackRange = 10;
        this.attackSpeed = 1f;
    }

    public override void Attack()
    {
        Debug.Log("Sword Attacking");
    }

    public override void Equip(PhotonView PV)
    {
        isEquiped = true;

        // equip when only in the equipment slots
        if (this.uiIndex < 0)
            NetworkCalls.Weapon.EquipSword(PV);
        else
            Debug.Log("Please drag this weapon into the equipment slots");
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
