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
        canDirectHit = true;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 15f,
            knockBackDist = 2f,
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
