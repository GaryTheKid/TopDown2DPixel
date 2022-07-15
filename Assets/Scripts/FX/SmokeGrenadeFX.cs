using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeFX : FXPlayer_Projectile
{
    [SerializeField] private GameObject ScreenSmokeTrigger;
    [SerializeField] private float throwingTorque;
    [SerializeField] private float activationTorque;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Rigidbody2D parentRb;

    private IEnumerator playSmoke_Co;
    private IEnumerator Co_playSmoke()
    {
        // get parent
        ProjectileWorld parent = GetComponentInParent<ProjectileWorld>();

        // add torque, let it spin
        rb.AddTorque(throwingTorque, ForceMode2D.Impulse);

        // stop parent expire coroutine
        parent.StopAllCoroutines();

        // wait for activation
        yield return new WaitForSecondsRealtime(parent.GetComponent<ProjectileWorld>().GetProjectile().activationTime);

        // activate trigger
        ScreenSmokeTrigger.SetActive(true);

        // add torque, let it spin
        rb.AddTorque(activationTorque, ForceMode2D.Impulse);

        // play sound fx
        var soundFX = GetComponentInChildren<AudioSource>();
        soundFX.PlayOneShot(soundFX.clip);

        // activate smoke particle
        smoke.Play();

        // wait expire
        yield return new WaitForSecondsRealtime(smoke.main.duration + smoke.main.startLifetimeMultiplier);

        // clear
        Destroy(parent.gameObject);        
    }

    private void Start()
    {
        PlayInitializationFX();
    }

    public override void PlayInitializationFX()
    {
        if (playSmoke_Co == null)
        {
            playSmoke_Co = Co_playSmoke();
            StartCoroutine(playSmoke_Co);
        }
    }

    public override void PlayStickFX()
    {
        
    }

    public override void PlayCollisionFX()
    {

    }

    public override void PlayHitFX()
    {

    }
}
