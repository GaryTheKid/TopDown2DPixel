using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun_SMAX : Gun
{
    public Gun_SMAX()
    {
        itemName = "SMAX shotgun";
        itemID = 20;
        amount = 1;
        itemType = ItemType.RangedWeapon;
        durability = 30;

        attackRange = 10f;
        attackSpeed = 3f;
        moveSlowDownModifier = 0.1f;
        moveSlowDownTime = 0.15f;
        accuracy = 0.9f;
        recoilForce = 6.5f;
        recoilTime = 0.03f;
        recoilRecoverTime = 0.12f;

        // projectile info
        projectile = new Bullet_Shotgun();
        projectile.spawnWeapon = this;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfGun_SMAX;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.gunSprite_SMAX;
    }

    public override Sprite GetDurabilitySprite()
    {
        return ItemAssets.itemAssets.ui_icon_ShotgunBullets;
    }

    public override void Attack(PhotonView attackerPV, Vector2 firePos, float fireDirDeg)
    {
        // shoot projectiles
        NetworkCalls.Weapon_Network.FireProjectile(attackerPV, firePos, fireDirDeg + 15f);
        NetworkCalls.Weapon_Network.FireProjectile(attackerPV, firePos, fireDirDeg + 5f);
        NetworkCalls.Weapon_Network.FireProjectile(attackerPV, firePos, fireDirDeg - 5f);
        NetworkCalls.Weapon_Network.FireProjectile(attackerPV, firePos, fireDirDeg - 15f);

        // play sfx
        NetworkCalls.Weapon_Network.PlayOneShotSFX_Projectile(attackerPV);
    }
}
