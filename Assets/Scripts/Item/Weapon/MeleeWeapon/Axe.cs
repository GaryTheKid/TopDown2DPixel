using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{
    public Axe()
    {
        this.itemName = "Axe";
        this.amount = 1;
        this.itemType = ItemType.MeleeWeapon;
        this.attackRange = 8f;
        this.attackSpeed = 1.2f;
        this.damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 20f,
            damageDelay = 0.2f,
            damageEffectTime = 0f,
            KnockBackDist = 0f,
        };
    }

    public Axe(int amount)
    {
        this.itemName = "Axe";
        this.amount = amount;
        this.itemType = ItemType.MeleeWeapon;
        this.attackRange = 8f;
        this.attackSpeed = 1.2f;
        this.damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 20f,
            damageDelay = 0.2f,
            damageEffectTime = 0f,
            KnockBackDist = 0f,
        };
    }

    public override void Attack(PhotonView attackerPV)
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
    
