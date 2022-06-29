using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_AK : Gun
{
    public Gun_AK()
    {
        itemName = "AK-47";
        itemID = 5;
        amount = 1;
        itemType = ItemType.RangedWeapon;
        durability = 30;

        attackRange = 10f;
        attackSpeed = 8f;
        moveSlowDownModifier = 0.15f;
        moveSlowDownTime = 0.20f;
        accuracy = 0.95f;
        recoilForce = 2f;
        recoilTime = 0.05f;
        recoilRecoverTime = 0.15f;

        // projectile info
        projectile = new Bullet_SemiAuto();
        projectile.spawnWeapon = this;
        projectile.speed = 20f;
        projectile.damageInfo.damageAmount = 20f;
        projectile.damageInfo.KnockBackDist = 3f;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfGun_AK;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.gunSprite_AK;
    }

    public override Sprite GetDurabilitySprite()
    {
        return ItemAssets.itemAssets.ui_icon_semiAutoBullets;
    }
}
