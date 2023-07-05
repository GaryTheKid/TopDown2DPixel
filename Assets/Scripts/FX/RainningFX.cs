using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RainningFX : MonoBehaviour
{
    // constant
    private const float RAINNING_TRANSITION_TIME = 5f;
    private const float PARTICLE_RAINNING_RATE = 150f;
    private const float PARTICLE_RIPPLE_RATE = 200f;

    // public field
    public ParticleSystem rainningParticle;
    public ParticleSystem rippleParticle;
    public Volume volume;
    public AudioSource rainningSFX;
    public Color grey;

    // private field
    private float timer;
    private ColorAdjustments colorAdjustments;
    private float volumeModifier;

    // cache coroutine
    private IEnumerator _co_rainning;

    private void Awake()
    {
        volume.profile.TryGet(out colorAdjustments);
    }

    public void StartRainning()
    {
        // clear previous coroutine
        if (_co_rainning != null)
        {
            StopCoroutine(_co_rainning);
            _co_rainning = null;
        }

        // start new coroutine
        _co_rainning = Co_StartRainning();
        StartCoroutine(_co_rainning);
    }

    public void StopRainning()
    {
        // clear previous coroutine
        if (_co_rainning != null)
        {
            StopCoroutine(_co_rainning);
            _co_rainning = null;
        }

        // start new coroutine
        _co_rainning = Co_StopRainning();
        StartCoroutine(_co_rainning);
    }

    public void SetVolumeModifier(float modifier)
    {
        volumeModifier = modifier;
        rainningSFX.volume = 1f * volumeModifier;
    }

    private IEnumerator Co_StartRainning() 
    {
        // ref emission
        var rainningEmission = rainningParticle.emission;
        var rippleEmission = rippleParticle.emission;

        // enable emission
        rainningParticle.Play();
        rippleParticle.Play();
        rainningSFX.Play();

        // transition
        while (timer < RAINNING_TRANSITION_TIME)
        {
            var ratio = timer / RAINNING_TRANSITION_TIME;
            var dt = Time.deltaTime;
            rainningEmission.rateOverTime = ratio * PARTICLE_RAINNING_RATE;
            rippleEmission.rateOverTime = ratio * PARTICLE_RIPPLE_RATE;
            rainningSFX.volume = ratio * 1f * volumeModifier;
            colorAdjustments.colorFilter.value = Color.Lerp(colorAdjustments.colorFilter.value, grey, dt);
            timer += dt;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Co_StopRainning()
    {
        // ref emission
        var rainningEmission = rainningParticle.emission;
        var rippleEmission = rippleParticle.emission;

        // transition
        while (timer > 0f)
        {
            var ratio = timer / RAINNING_TRANSITION_TIME;
            var dt = Time.deltaTime;
            rainningEmission.rateOverTime = ratio * PARTICLE_RAINNING_RATE;
            rippleEmission.rateOverTime = ratio * PARTICLE_RIPPLE_RATE;
            rainningSFX.volume = ratio * 1f * volumeModifier;
            colorAdjustments.colorFilter.value = Color.Lerp(colorAdjustments.colorFilter.value, Color.white, dt);
            timer -= dt;
            yield return new WaitForEndOfFrame();
        }

        // disable emission
        rainningParticle.Stop();
        rippleParticle.Stop();
        rainningSFX.Stop();
    }
}
