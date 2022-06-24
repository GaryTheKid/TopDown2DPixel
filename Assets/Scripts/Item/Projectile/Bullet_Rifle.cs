using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Rifle : Projectile
{
    public Bullet_Rifle()
    {
        speed = 28f;
        maxDist = 100f;
        explosiveRadius = 0f;
        lifeTime = 1f;
        isSticky = false;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 60f,
            damageDelay = 0.2f,
            damageEffectTime = 0f,
            KnockBackDist = 5f,
        };
    }

    public override Transform GetProjectilePrefab()
    {
        return ItemAssets.itemAssets.projBullet_Rifle;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.bulletSprite;
    }
}
