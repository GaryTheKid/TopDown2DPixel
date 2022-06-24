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
        parent.DisablePhysics();
        parent.StopAllCoroutines();

        // start animation
        animator.SetTrigger("Hit");

        // play soundFX
        var soundFX = GetComponentInChildren<AudioSource>();
        soundFX.pitch = Random.Range(1f, 2f);
        soundFX.PlayOneShot(soundFX.clip);
    }

    public override void PlayHitFX()
    {
        // get parent
        ProjectileWorld parent = GetComponentInParent<ProjectileWorld>();
        parent.DisablePhysics();
        parent.StopAllCoroutines();

        // start animation
        animator.SetTrigger("Hit");

        // play soundFX
        var soundFX = GetComponentInChildren<AudioSource>();
        soundFX.pitch = Random.Range(1f, 2f);
        soundFX.PlayOneShot(soundFX.clip);
    }

    public void DestroySelf()
    {
        Destroy(transform.parent.gameObject);
    }
}
