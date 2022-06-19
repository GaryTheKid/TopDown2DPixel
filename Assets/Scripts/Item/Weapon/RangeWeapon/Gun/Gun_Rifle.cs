using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Rifle : Gun
{
    public Gun_Rifle()
    {
        itemName = "Gun_Rifle";
        itemID = 11;
        amount = 1;
        itemType = ItemType.RangedWeapon;
        attackRange = 10f;
        attackSpeed = 0.4f;
        attackMoveSlowRate = 0.15f;
        accuracy = 0.9f;
        recoilForce = 0.45f;
        recoilTime = 0.05f;
        recoilRecoverTime = 0.05f;

        // projectile info
        projectile = new Bullet();
        projectile.spawnWeapon = this;
        projectile.damageInfo.damageAmount = 60f;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfGun_Rifle;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.gunSprite_Rifle;
    }
}
