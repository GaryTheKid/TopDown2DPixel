using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bow : Weapon
{
    public Bow()
    {
        itemName = "Bow";
        itemID = 2;
        amount = 1;
        itemType = ItemType.ChargableRangedWeapon;
        attackSpeed = 1.5f;
        moveSlowDownModifier = 0.8f;
        moveSlowDownTime = 0.3f;
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
        NetworkCalls.Weapon_Network.ChargeWeapon(PV);
    }

    public override void Attack(PhotonView attackerPV, Vector2 firePos, float fireDirDeg)
    {
        // shoot projectiles
        NetworkCalls.Weapon_Network.FireChargedProjectile(attackerPV, firePos, fireDirDeg);
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
