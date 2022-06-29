using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet_SemiAuto : Projectile
{
    public Bullet_SemiAuto()
    {
        projectileID = 1;

        speed = 18f;
        maxDist = 100f;
        explosiveRadius = 0f;
        lifeTime = 1.2f;
        isSticky = false;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 15f,
            damageDelay = 0.2f,
            damageEffectTime = 0f,
            KnockBackDist = 2f,
        };
    }

    public override Transform GetProjectilePrefab()
    {
        return ItemAssets.itemAssets.projBullet_SemiAuto;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.ui_icon_semiAutoBullets;
    }
}
