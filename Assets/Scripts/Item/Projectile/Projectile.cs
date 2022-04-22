using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Projectile : Item
{
    public float speed;
    public float maxDist;
    public float lifeTime;
    public float accuracy;
    public float explosiveRadius;

    public abstract void Fire(PhotonView PV, Vector2 dir);
    public abstract Transform GetProjectilePrefab();
}
