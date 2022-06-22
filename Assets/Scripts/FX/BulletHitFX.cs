using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitFX : FXPlayer_Projectile
{
    [SerializeField] private Animator animator;

    public override void PlayInitializationFX()
    {

    }

    public override void PlayStickFX()
    {

    }

    public override void PlayCollisionFX()
    {
        // get parent
        ProjectileWorld parent = GetComponentInParent<ProjectileWorld>();
        parent.RemovePhysics();
        parent.StopAllCoroutines();

        // start animation
        animator.SetTrigger("Hit");
    }

    public override void PlayHitFX()
    {
        // get parent
        ProjectileWorld parent = GetComponentInParent<ProjectileWorld>();
        parent.RemovePhysics();
        parent.StopAllCoroutines();

        // start animation
        animator.SetTrigger("Hit");
    }

    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }
}
