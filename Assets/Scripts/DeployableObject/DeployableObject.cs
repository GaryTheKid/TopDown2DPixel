using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeployableObject : Item
{
    public short DeployableID;
    public DamageInfo damageInfo;
    public float lifeTime;
    public float activationTime;
    public float activationRadius;
    public float detectionRadius;
    public float explosiveRadius;
    public DeployableWeapon spawnWeapon;
    public bool isDeactivatable;

    public abstract Transform GetDeployableObjectPrefab();
}
