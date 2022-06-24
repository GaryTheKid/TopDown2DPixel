using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_M4 : Gun
{
    public Gun_M4()
    {
        itemName = "M4-A1";
        itemID = 6;
        amount = 1;
        itemType = ItemType.RangedWeapon;
        attackRange = 10f;
        attackSpeed = 9f;
        moveSlowDownModifier = 0.1f;
        moveSlowDownTime = 0.15f;
        accuracy = 0.98f;
        recoilForce = 0.2f;
        recoilTime = 0.03f;
        recoilRecoverTime = 0.12f;

        // projectile info
        projectile = new Bullet();
        projectile.spawnWeapon = this;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfGun_M4;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.gunSprite_M4;
    }
}