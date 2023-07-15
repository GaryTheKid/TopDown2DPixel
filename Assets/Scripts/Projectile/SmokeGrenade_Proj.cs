using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenade_Proj : Projectile
{
    public SmokeGrenade_Proj()
    {
        projectileID = 3;

        speed = 5f;
        maxDist = 100f;
        explosiveRadius = 0f;
        lifeTime = 10f;
        activationTime = 3f;
        isSticky = false;
        canDirectHit = false;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 0f,
            knockBackDist = 0f,
        };
    }

    public override Transform GetProjectilePrefab()
    {
        return ItemAssets.itemAssets.projSmokeGrenade;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.smokeGrenadeSprite;
    }
}