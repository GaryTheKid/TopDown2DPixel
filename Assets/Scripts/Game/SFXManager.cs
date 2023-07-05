using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    // Singleton instance
    public static SFXManager singleton;

    [SerializeField] private List<AudioSource> _activeSFXs;
    [SerializeField] private WindyFX _weatherSFX_Windy;
    [SerializeField] private RainningFX _weatherSFX_Rainning;

    private void Awake()
    {
        // Ensure only one instance of BGMManager exists
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        foreach (var sfx in FindObjectsOfType<AudioSource>())
        {
            if (sfx.gameObject.name != "BGMManager")
            {
                sfx.volume = PlayerSettings.singleton.FXVolume;
                _activeSFXs.Add(sfx);
            }
        }

        foreach (Transform portal in GameObject.Find("Portals").transform)
        {
            var sfx1 = portal.GetComponent<Teleport_Single>().sfx_Portal;
            sfx1.volume = PlayerSettings.singleton.FXVolume;
            _activeSFXs.Add(sfx1);

            var sfx2 = portal.GetComponent<Teleport_Single>().sfx_Exit;
            sfx2.volume = PlayerSettings.singleton.FXVolume;
            _activeSFXs.Add(sfx2);
        }
    }

    public void SetWeatherSFXs(WindyFX windy, RainningFX rainning)
    {
        _weatherSFX_Windy = windy;
        _weatherSFX_Windy.SetVolumeModifier(PlayerSettings.singleton.FXVolume);
        _weatherSFX_Rainning = rainning;
        _weatherSFX_Rainning.SetVolumeModifier(PlayerSettings.singleton.FXVolume);
    }

    public void Add(AudioSource sfx)
    {
        sfx.volume = PlayerSettings.singleton.FXVolume;
        _activeSFXs.Add(sfx);
    }

    public void Remove(AudioSource sfx)
    {
        _activeSFXs.Remove(sfx);
    }

    public void SetAllSFXVolume(float volume)
    {
        foreach (var sfx in _activeSFXs)
        {
            if (sfx != null)
            {
                sfx.volume = volume;
            }
        }

        if (_weatherSFX_Windy != null)
        {
            _weatherSFX_Windy.SetVolumeModifier(PlayerSettings.singleton.FXVolume);
        }

        if (_weatherSFX_Rainning != null)
        {
            _weatherSFX_Rainning.SetVolumeModifier(PlayerSettings.singleton.FXVolume);
        }
    }
}
