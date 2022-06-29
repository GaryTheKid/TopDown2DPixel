using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Pistol : Gun
{
    public Gun_Pistol()
    {
        itemName = "Pistol";
        itemID = 10;
        amount = 1;
        itemType = ItemType.RangedWeapon;
        durability = 25;

        attackRange = 10f;
        attackSpeed = 2.5f;
        moveSlowDownModifier = 0.6f;
        moveSlowDownTime = 0.1f;
        accuracy = 0.95f;
        recoilForce = 0.5f;
        recoilTime = 0.03f;
        recoilRecoverTime = 0.03f;

        // projectile info
        projectile = new Bullet_SemiAuto();
        projectile.spawnWeapon = this;
        projectile.damageInfo.damageAmount = 15f;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfGun_Pistol;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.gunSprite_Pistol;
    }

    public override Sprite GetDurabilitySprite()
    {
        return ItemAssets.itemAssets.ui_icon_pistolBullets;
    }
}
