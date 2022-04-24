using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Projectile : Item
{
    public Weapon spawnWeapon;
    public DamageInfo damageInfo;
    public float speed;
    public float maxDist;
    public float lifeTime;
    public float accuracy;
    public float explosiveRadius;
    public float explosiveTime;
    public bool isSticky;

    public abstract Transform GetProjectilePrefab();
}
