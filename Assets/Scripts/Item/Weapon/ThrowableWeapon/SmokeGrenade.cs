using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SmokeGrenade : Weapon
{
    public SmokeGrenade()
    {
        itemName = "Smoke Grenade";
        itemID = 13;
        amount = 1;
        itemType = ItemType.ThrowableWeapon;
        attackSpeed = 1.5f;
        attackMoveSlowRate = 0.8f;
        accuracy = 1f;
        maxChargeTier = 3;
        chargeSpeed = 6f;
        chargeMoveSlowRate = 0.5f;

        // projectile info
        projectile = new SmokeGrenade_Proj();
        projectile.spawnWeapon = this;
    }

    public SmokeGrenade(short amount)
    {
        itemName = "Smoke Grenade";
        itemID = 13;
        this.amount = amount;
        itemType = ItemType.ThrowableWeapon;
        attackSpeed = 1.5f;
        attackMoveSlowRate = 0.8f;
        accuracy = 1f;
        maxChargeTier = 3;
        chargeSpeed = 6f;
        chargeMoveSlowRate = 0.5f;

        // projectile info
        projectile = new SmokeGrenade_Proj();
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

    public override bool IsStackable()
    {
        return true;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfSmokeGrenade;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.SmokeGrenadeSprite;
    }
}
