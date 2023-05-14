using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitFX : FXPlayer_Projectile
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject hitMark;

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
        sprite.SetActive(false);
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
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
        sprite.SetActive(false);
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        animator.SetTrigger("Hit");

        // show hit mark
        var hitPos = transform.position + transform.forward.normalized * 0.25f;
        Instantiate(hitMark, hitPos, Quaternion.identity, GameManager.singleton.FXParent);

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
