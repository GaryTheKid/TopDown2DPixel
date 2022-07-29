using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ImpactGrenade : Weapon
{
    public ImpactGrenade()
    {
        itemName = "Impact Grenade";
        itemID = 15;
        amount = 1;
        durability = 999;
        itemType = ItemType.ThrowableWeapon;
        attackSpeed = 1.5f;
        moveSlowDownModifier = 0.8f;
        moveSlowDownTime = 0.3f;
        accuracy = 1f;
        maxChargeTier = 3;
        chargeSpeed = 6f;
        chargeMoveSlowRate = 0.5f;

        // projectile info
        projectile = new ImpactGrenade_Proj();
        projectile.spawnWeapon = this;
    }

    public ImpactGrenade(short amount)
    {
        itemName = "Impact Grenade";
        itemID = 15;
        this.amount = amount;
        durability = 999;
        itemType = ItemType.ThrowableWeapon;
        attackSpeed = 1.5f;
        moveSlowDownModifier = 0.8f;
        moveSlowDownTime = 0.3f;
        accuracy = 1f;
        maxChargeTier = 3;
        chargeSpeed = 6f;
        chargeMoveSlowRate = 0.5f;

        // projectile info
        projectile = new ImpactGrenade_Proj();
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
        return ItemAssets.itemAssets.pfImpactGrenade;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.impactGrenadeSprite;
    }
}
