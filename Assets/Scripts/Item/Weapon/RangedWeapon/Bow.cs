using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bow : Weapon
{
    public Bow()
    {
        this.itemName = "Bow";
        this.amount = 1;
        this.itemType = ItemType.RangedWeapon;
        this.attackSpeed = 1f;
        this.maxChargeTier = 3;
        this.chargeSpeed = 3f;
        this.damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 15f,
            damageDelay = 0.2f,
            damageEffectTime = 0f,
            KnockBackDist = 2f,
        };
    }

    public override void Charge(PhotonView PV)
    {
        // charge weapon
        NetworkCalls.Character.ChargeWeapon(PV);
    }

    public override void Attack(PhotonView attackerPV)
    {
        // shoot projectiles
        NetworkCalls.Character.FireProjectile(attackerPV);
    }

    public override void Equip(PhotonView PV)
    {
        isEquiped = true;

        // equip when only in the equipment slots
        if (this.uiIndex < 0)
            NetworkCalls.Weapon.EquipBow(PV);
        else
            Debug.Log("Please drag this weapon into the equipment slots");
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfBow;
    }

    public override Sprite GetSprite() 
    {
        return ItemAssets.itemAssets.bowSprite;
    }
}
