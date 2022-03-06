using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : Item
{
    public abstract void Fire(Vector2 dir);
    public abstract void Effect();
    public abstract Transform GetProjectilePrefab();
}
