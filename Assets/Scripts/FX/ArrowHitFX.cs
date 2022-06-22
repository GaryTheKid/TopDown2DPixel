using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHitFX : FXPlayer_Projectile
{
    public override void PlayInitializationFX()
    {
        
    }

    public override void PlayCollisionFX()
    {

    }

    public override void PlayHitFX()
    {
        
    }

    public override void PlayStickFX()
    {
        GetComponentInParent<ProjectileWorld>().RemovePhysics();
    }
}
