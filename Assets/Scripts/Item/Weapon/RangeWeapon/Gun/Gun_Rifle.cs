using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Rifle : Gun
{
    public Gun_Rifle()
    {
        itemName = "Rifle";
        itemID = 11;
        amount = 1;
        itemType = ItemType.RangedWeapon;
        durability = 15;

        attackRange = 10f;
        attackSpeed = 0.7f;
        moveSlowDownModifier = 0.15f;
        moveSlowDownTime = 0.8f;
        accuracy = 1f;
        recoilForce = 0.45f;
        recoilTime = 0.05f;
        recoilRecoverTime = 0.05f;

        // projectile info
        projectile = new Bullet_Rifle();
        projectile.spawnWeapon = this;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfGun_Rifle;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.gunSprite_Rifle;
    }

    public override Sprite GetDurabilitySprite()
    {
        return ItemAssets.itemAssets.ui_icon_rifleBullet;
    }
}
