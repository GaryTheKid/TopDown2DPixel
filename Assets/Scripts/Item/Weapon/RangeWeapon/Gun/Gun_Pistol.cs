using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Pistol : Gun
{
    public Gun_Pistol()
    {
        itemName = "Gun_Pistol";
        itemID = 10;
        amount = 1;
        itemType = ItemType.RangedWeapon;
        attackRange = 10f;
        attackSpeed = 2.5f;
        attackMoveSlowRate = 0.5f;
        accuracy = 0.9f;
        recoilForce = 0.35f;
        recoilTime = 0.03f;
        recoilRecoverTime = 0.03f;

        // projectile info
        projectile = new Bullet();
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
}
