using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : Item
{
    public float speed;
    public float maxDist;
    public float explosiveRadius;

    public abstract void Fire(Vector2 dir);
    public abstract Transform GetProjectilePrefab();
}
