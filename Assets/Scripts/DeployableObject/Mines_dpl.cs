using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mines_dpl : DeployableObject
{
    public Mines_dpl()
    {
        DeployableID = 0;
        lifeTime = 60f;
        activationTime = 1f;
        activationRadius = 0.5f;
        detectionRadius = 1f;
        damageInfo = new DamageInfo
        {
            damageType = DamageInfo.DamageType.Physics,
            damageAmount = 80f,
            damageDelay = 0f,
            damageEffectTime = 0f,
            KnockBackDist = 8f,
        };
    }

    public override Transform GetDeployableObjectPrefab()
    {
        return ItemAssets.itemAssets.deployableObj_Mine;
    }
}
