using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEGrenade_proj : Projectile
{
    public HEGrenade_proj()
    {
        projectileID = 4;

        speed = 5f;
        maxDist = 100f;
        explosiveRadius = 6.5f;
        lifeTime = 6f;
        activationTime = 2f;
        isSticky = false;
        isExplosive = true;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 100f,
            damageDelay = 0f,
            damageEffectTime = 0f,
            KnockBackDist = 5f,
        };
    }

    public override Transform GetProjectilePrefab()
    {
        return ItemAssets.itemAssets.projHEGrenade;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.HEGrenadeSprite;
    }
}
