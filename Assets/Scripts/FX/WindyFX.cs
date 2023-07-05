/* Last Edition Date: 02/10/2023
 * Author: Chongyang Wang
 * Collaborators: 
 * References:
 * 
 * Description: 
 *   The windy weather fx.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindyFX : MonoBehaviour
{
    // constant
    private const float WINDING_TRANSITION_TIME = 3f;
    private const float PARTICLE_WINDING_RATE = 16f;
    private const float PARTICLE_LEAVES_RATE = 14f;

    // public field
    public enum WindDir
    {
        East,
        West,
        North,
        South
    }
    public ParticleSystem windyParticle;
    public ParticleSystem leavesParticle;
    public AudioSource windySFX;

    // private field
    [SerializeField] private Transform _windPos_East;
    [SerializeField] private Transform _windPos_West;
    [SerializeField] private Transform _windPos_North;
    [SerializeField] private Transform _windPos_South;
    private float timer;
    private float volumeModifier;

    // cache coroutine
    private IEnumerator _co_windy;

    public void StartWinding(WindDir windDir)
    {
        // set winding pos and dir
        switch (windDir)
        {
            case WindDir.East:
                transform.position = _windPos_East.position;
                transform.eulerAngles = new Vector3(0f, 0f, 0f);
                break;
            case WindDir.West:
                transform.position = _windPos_West.position;
                transform.eulerAngles = new Vector3(0f, 0f, 180f);
                break;
            case WindDir.North:
                transform.position = _windPos_North.position;
                transform.eulerAngles = new Vector3(0f, 0f, 90f);
                break;
            case WindDir.South:
                transform.position = _windPos_South.position;
                transform.eulerAngles = new Vector3(0f, 0f, -90f);
                break;
        }

        // clear previous coroutine
        if (_co_windy != null)
        {
            StopCoroutine(_co_windy);
            _co_windy = null;
        }

        // start new coroutine
        _co_windy = Co_StartWinding();
        StartCoroutine(_co_windy);
    }

    public void StopWinding()
    {
        // clear previous coroutine
        if (_co_windy != null)
        {
            StopCoroutine(_co_windy);
            _co_windy = null;
        }

        // start new coroutine
        _co_windy = Co_StopWinding();
        StartCoroutine(_co_windy);
    }

    public void SetVolumeModifier(float modifier)
    {
        volumeModifier = modifier;
        windySFX.volume = 1f * volumeModifier;
    }

    private IEnumerator Co_StartWinding()
    {
        // ref emission
        var windyEmission = windyParticle.emission;
        var leavesEmission = leavesParticle.emission;

        // enable emission
        windyParticle.Play();
        leavesParticle.Play();
        windySFX.Play();

        // transition
        while (timer < WINDING_TRANSITION_TIME)
        {
            var ratio = timer / WINDING_TRANSITION_TIME;
            var dt = Time.deltaTime;
            windyEmission.rateOverTime = ratio * PARTICLE_WINDING_RATE;
            leavesEmission.rateOverTime = ratio * PARTICLE_LEAVES_RATE;
            windySFX.volume = ratio * 1f * volumeModifier;
            timer += dt;
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator Co_StopWinding()
    {
        // ref emission
        var windyEmission = windyParticle.emission;
        var leavesEmission = leavesParticle.emission;

        // transition
        while (timer > 0f)
        {
            var ratio = timer / WINDING_TRANSITION_TIME;
            var dt = Time.deltaTime;
            windyEmission.rateOverTime = ratio * PARTICLE_WINDING_RATE;
            leavesEmission.rateOverTime = ratio * PARTICLE_LEAVES_RATE;
            windySFX.volume = ratio * 1f * volumeModifier;
            timer -= dt;
            yield return new WaitForEndOfFrame();
        }

        // disable emission
        windyParticle.Stop();
        leavesParticle.Stop();
        windySFX.Stop();
    }
}
