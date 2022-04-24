using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bow : Weapon
{
    public Bow()
    {
        itemName = "Bow";
        amount = 1;
        itemType = ItemType.RangedWeapon;
        attackSpeed = 1.5f;
        maxChargeTier = 3;
        chargeSpeed = 6f;
        chargeMoveSlowRate = 0.5f;

        // projectile info
        projectile = new Arrow();
        projectile.spawnWeapon = this;
    }

    public override void Charge(PhotonView PV)
    {
        // charge weapon
        NetworkCalls.Character.ChargeWeapon(PV);
    }

    public override void Attack(PhotonView attackerPV)
    {
        // shoot projectiles
        NetworkCalls.Character.FireChargedProjectile(attackerPV);
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
