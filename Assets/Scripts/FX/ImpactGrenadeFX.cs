using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactGrenadeFX : FXPlayer_Projectile
{
    [SerializeField] private GameObject explosionImpactTrigger;
    [SerializeField] private float throwingTorque;
    [SerializeField] private Rigidbody2D rb;

    private IEnumerator explodeHE_Co;
    private IEnumerator Co_explodeHE()
    {
        // get parent
        ProjectileWorld parent = GetComponentInParent<ProjectileWorld>();

        // add torque, let it spin
        rb.AddTorque(throwingTorque, ForceMode2D.Impulse);

        // stop parent expire coroutine
        parent.StopAllCoroutines();

        // wait for activation
        yield return new WaitForSecondsRealtime(parent.GetComponent<ProjectileWorld>().GetProjectile().activationTime);

        // play sound fx
        var soundFX = GetComponentInChildren<AudioSource>();
        soundFX.PlayOneShot(soundFX.clip);

        // explode, calculate damage
        explosionImpactTrigger.SetActive(true);
    }

    private void Start()
    {
        PlayInitializationFX();
    }

    public override void PlayInitializationFX()
    {
        if (explodeHE_Co == null)
        {
            explodeHE_Co = Co_explodeHE();
            StartCoroutine(explodeHE_Co);
        }
    }

    public override void PlayStickFX()
    {

    }

    public override void PlayCollisionFX()
    {
        // stop parent expire coroutine
        GetComponentInParent<ProjectileWorld>().StopAllCoroutines();

        // play sound fx
        var soundFX = GetComponentInChildren<AudioSource>();
        soundFX.PlayOneShot(soundFX.clip);

        // explode, calculate damage
        explosionImpactTrigger.SetActive(true);

        // stop coroutine
        StopAllCoroutines();
    }

    public override void PlayHitFX()
    {
        // stop parent expire coroutine
        GetComponentInParent<ProjectileWorld>().StopAllCoroutines();

        // play sound fx
        var soundFX = GetComponentInChildren<AudioSource>();
        soundFX.PlayOneShot(soundFX.clip);

        // explode, calculate damage
        explosionImpactTrigger.SetActive(true);

        // stop coroutine
        StopAllCoroutines();
    }
}
