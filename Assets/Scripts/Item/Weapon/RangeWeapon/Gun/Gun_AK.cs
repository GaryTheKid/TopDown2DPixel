using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_AK : Gun
{
    public Gun_AK()
    {
        itemName = "Gun_AK";
        amount = 1;
        itemType = ItemType.RangedWeapon;
        attackRange = 10f;
        attackSpeed = 8f;
        attackMoveSlowRate = 0.15f;
        accuracy = 0.9f;
        recoilForce = 0.6f;
        recoilTime = 0.05f;
        recoilRecoverTime = 0.15f;

        // projectile info
        projectile = new Bullet();
        projectile.spawnWeapon = this;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfGun_AK;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.gunSprite_AK;
    }
}
