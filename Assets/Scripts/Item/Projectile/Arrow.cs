using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : Projectile
{
    public Arrow()
    {
        speed = 4f;
        maxDist = 100f;
        explosiveRadius = 0f;
        lifeTime = 5f;
        accuracy = 1f;
        isSticky = true;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 20f,
            damageDelay = 0.2f,
            damageEffectTime = 0f,
            KnockBackDist = 2f,
        };
    }

    public override void Fire(PhotonView PV)
    {
        // fire projectile towards a direction
        NetworkCalls.Character.ChargeWeapon(PV);
    }

    public override Transform GetProjectilePrefab()
    {
        return ItemAssets.itemAssets.pfArrow;
    }

    public override Sprite GetSprite()
    {
        return ItemAssets.itemAssets.arrowSprite;
    }

    public override string GetProjectilePrefabPath()
    {
        return "Item/Projectile/Arrow";
    }
}