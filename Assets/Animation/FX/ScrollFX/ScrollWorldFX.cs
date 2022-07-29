using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollWorldFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem _unleashFX;

    public void UnleashFX()
    {
        _unleashFX.Play();
    }
}
