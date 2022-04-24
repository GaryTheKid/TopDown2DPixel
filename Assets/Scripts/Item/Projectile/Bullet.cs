using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bullet : Projectile
{
    public Bullet()
    {
        speed = 15f;
        maxDist = 100f;
        explosiveRadius = 0f;
        lifeTime = 3f;
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
        return ItemAssets.itemAssets.pfBullet;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.bulletSprite;
    }
}
