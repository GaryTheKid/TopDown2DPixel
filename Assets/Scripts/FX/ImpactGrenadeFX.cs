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
        // add to sfx manager
        var soundFX = GetComponentInChildren<AudioSource>();
        SFXManager.singleton.Add(soundFX);

        // get parent
        ProjectileWorld parent = GetComponentInParent<ProjectileWorld>();

        // add torque, let it spin
        rb.AddTorque(throwingTorque, ForceMode2D.Impulse);

        // stop parent expire coroutine
        parent.StopAllCoroutines();

        // wait for activation
        yield return new WaitForSecondsRealtime(parent.GetComponent<ProjectileWorld>().GetProjectile().activationTime);

        // play sound fx
        soundFX.PlayOneShot(soundFX.clip);

        // explode, calculate damage
        explosionImpactTrigger.SetActive(true);
    }

    private void Start()
    {
        PlayInitializationFX();
    }

    private void OnDestroy()
    {
        SFXManager.singleton.Remove(GetComponentInChildren<AudioSource>());
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
        // add to sfx manager
        var soundFX = GetComponentInChildren<AudioSource>();
        SFXManager.singleton.Add(soundFX);

        // stop parent expire coroutine
        GetComponentInParent<ProjectileWorld>().StopAllCoroutines();

        // play sound fx
        soundFX.PlayOneShot(soundFX.clip);

        // explode, calculate damage
        explosionImpactTrigger.SetActive(true);

        // stop coroutine
        StopAllCoroutines();
    }

    public override void PlayHitFX()
    {
        // add to sfx manager
        var soundFX = GetComponentInChildren<AudioSource>();
        SFXManager.singleton.Add(soundFX);

        // stop parent expire coroutine
        GetComponentInParent<ProjectileWorld>().StopAllCoroutines();

        // play sound fx
        soundFX.PlayOneShot(soundFX.clip);

        // explode, calculate damage
        explosionImpactTrigger.SetActive(true);

        // stop coroutine
        StopAllCoroutines();
    }
}
