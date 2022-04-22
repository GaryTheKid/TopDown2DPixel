using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Arrow : Projectile
{
    public Arrow()
    {
        speed = 10f;
        maxDist = 100f;
        explosiveRadius = 0f;
        lifeTime = 5f;
        accuracy = 1f;
    }

    public override void Fire(PhotonView PV, Vector2 dir)
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
}
