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
        itemType = ItemType.ChargableRangedWeapon;
        attackSpeed = 1.5f;
        attackMoveSlowRate = 0.8f;
        accuracy = 1f;
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

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfBow;
    }

    public override Sprite GetSprite() 
    {
        return ItemAssets.itemAssets.bowSprite;
    }
}
