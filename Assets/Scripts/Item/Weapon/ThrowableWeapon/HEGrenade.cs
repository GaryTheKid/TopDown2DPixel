/* Last Edition: 07/14/2022
 * Author: Chongyang Wang
 * Collaborators: 
 * 
 * Description: 
 *   Item -> Weapon -> HEGrenade.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HEGrenade : Weapon
{
    public HEGrenade()
    {
        itemName = "HE Grenade";
        itemID = 14;
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
        projectile = new HEGrenade_proj();
        projectile.spawnWeapon = this;
    }

    public HEGrenade(short amount)
    {
        itemName = "HE Grenade";
        itemID = 14;
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
        projectile = new HEGrenade_proj();
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
        return ItemAssets.itemAssets.pfHEGrenade;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.HEGrenadeSprite;
    }
}
