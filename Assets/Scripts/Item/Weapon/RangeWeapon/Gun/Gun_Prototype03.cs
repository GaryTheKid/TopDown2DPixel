/* Last Edition Date: 02/05/2023
 * Author: Chongyang Wang
 * Collaborators: 
 * Reference: 
 * 
 * Description: 
 *   Item - Weapon - Gun - Prototype03
 * Last Edition:
 *   Change KnockBackDist 3 -> 1.5
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_Prototype03 : Gun
{
    public Gun_Prototype03()
    {
        itemName = "Prototype-03";
        itemID = 21;
        amount = 1;
        itemType = ItemType.RangedWeapon;
        cursorType = CursorType.SemiAuto;
        durability = 10;

        attackRange = 15f;
        attackSpeed = 2.5f;
        moveSlowDownModifier = 0.3f;
        moveSlowDownTime = 0.20f;
        accuracy = 1f;
        recoilForce = 0f;
        recoilTime = 0.01f;
        recoilRecoverTime = 0.01f;

        // projectile info
        projectile = new Bullet_Railgun();
        projectile.spawnWeapon = this;
        projectile.speed = 30f;
        projectile.damageInfo.damageAmount = 30f;
        projectile.damageInfo.KnockBackDist = 1.5f;
    }

    public override Transform GetEquipmentPrefab()
    {
        return ItemAssets.itemAssets.pfGun_Prototype03;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.gunSprite_Prototype03;
    }

    public override Sprite GetDurabilitySprite()
    {
        return ItemAssets.itemAssets.ui_icon_railGunBullet;
    }
}
