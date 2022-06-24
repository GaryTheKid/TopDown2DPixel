using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHitFX : FXPlayer_Projectile
{
    [SerializeField] private Transform SpriteAnchor;

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
        GetComponentInParent<ProjectileWorld>().DisablePhysics();
        SpriteAnchor.GetComponent<Animator>().enabled = true;
        GetComponentInParent<TrailRenderer>().enabled = false;
        var soundFX = GetComponentInChildren<AudioSource>();
        soundFX.PlayOneShot(soundFX.clip);
    }
}
