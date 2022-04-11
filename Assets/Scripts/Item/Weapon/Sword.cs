using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Sword : Weapon
{
    public Sword()
    {
        this.itemName = "Sword";
        this.amount = 1;
        this.itemType = ItemType.MeleeWeapon;
        this.attackRange = 10f;
        this.attackSpeed = 1f;
        this.damageInfo = new DamageInfo 
        { 
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 15f,
            damageDelay = 0.2f,
            damageEffectTime = 0f,
            KnockBackDist = 0f,
        };
    }

    public Sword(int amount)
    {
        this.itemName = "Sword";
        this.amount = amount;
        this.itemType = ItemType.MeleeWeapon;
        this.attackRange = 10f;
        this.attackSpeed = 1f;
        this.damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 15f,
            damageDelay = 0.2f,
            damageEffectTime = 0f,
            KnockBackDist = 0f,
        };
    }

    public override void Attack(PhotonView attackerPV)
    {
        // play the animation at userTransform
        NetworkCalls.Character.FireWeapon(attackerPV);

        // deal damage to all targets
        NetworkCalls.Character.DealDamage(attackerPV);
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
