using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // singleton
    public static AudioManager audioManager;

    public List<AudioClip> soundtracks;
    private AudioSource audioPlayer;

    private void Awake()
    {
        audioManager = this;
        audioPlayer = GetComponent<AudioSource>();
    }
}
