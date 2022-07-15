using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactGrenade_Proj : Projectile
{
    public ImpactGrenade_Proj()
    {
        projectileID = 5;

        speed = 5f;
        maxDist = 100f;
        explosiveRadius = 5f;
        lifeTime = 6f;
        activationTime = 2f;
        isSticky = false;
        isExplosive = true;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 80f,
            damageDelay = 0f,
            damageEffectTime = 0f,
            KnockBackDist = 5f,
        };
    }

    public override Transform GetProjectilePrefab()
    {
        return ItemAssets.itemAssets.projImpactGrenade;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.ImpactGrenadeSprite;
    }
}
