using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet_Shotgun : Projectile
{
    public Bullet_Shotgun()
    {
        projectileID = 6;

        speed = 18f;
        maxDist = 100f;
        explosiveRadius = 0f;
        lifeTime = 0.4f;
        isSticky = false;
        canDirectHit = true;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 10f,
            damageDelay = 0.2f,
            damageEffectTime = 0f,
            KnockBackDist = 2.5f,
        };
    }

    public override Transform GetProjectilePrefab()
    {
        return ItemAssets.itemAssets.projBullet_Shotgun;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.ui_icon_ShotgunBullets;
    }
}