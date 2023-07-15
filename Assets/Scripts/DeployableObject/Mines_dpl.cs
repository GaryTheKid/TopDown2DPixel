using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines_dpl : DeployableObject
{
    public Mines_dpl()
    {
        DeployableID = 0;
        lifeTime = 30f;
        activationTime = 1f;
        activationRadius = 0.5f;
        detectionRadius = 1f;
        explosiveRadius = 4f;
        isDeactivatable = false;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 80f,
            knockBackDist = 8f,
        };
    }

    public override Transform GetDeployableObjectPrefab()
    {
        return ItemAssets.itemAssets.deployableObj_Mine;
    }
}
