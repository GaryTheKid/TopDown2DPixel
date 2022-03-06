using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCalls;

public class Axe : Weapon
{
    public Axe()
    {
        this.itemName = "Axe";
        this.amount = 1;
        this.itemType = ItemType.MeleeWeapon;
        this.attackDmg = 20f;
        this.attackRange = 8f;
        this.attackSpeed = 1.2f;
    }

    public Axe(int amount)
    {
        this.itemName = "Axe";
        this.amount = amount;
        this.itemType = ItemType.MeleeWeapon;
        this.attackDmg = 20f;
        this.attackRange = 8f;
        this.attackSpeed = 1.2f;
    }

    public override void Attack()
    {
        Debug.Log("Sword Attacking");
    }

    public override void Equip(PhotonView PV)
    {
        NetworkCalls.Weapon.EquipAxe(PV);
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfAxe;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.axeSprite;
    }

    public override bool IsStackable()
    {
        return false;
    }
}
    
