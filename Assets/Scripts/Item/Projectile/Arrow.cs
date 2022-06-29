using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : Projectile
{
    public Arrow()
    {
        projectileID = 0;

        speed = 7f;
        maxDist = 100f;
        explosiveRadius = 0f;
        lifeTime = 1.8f;
        isSticky = true;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 60f,
            damageDelay = 0.2f,
            damageEffectTime = 0f,
            KnockBackDist = 2f,
        };
    }

    public override Transform GetProjectilePrefab()
    {
        return ItemAssets.itemAssets.projArrow;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.ui_icon_arrow;
    }
}
