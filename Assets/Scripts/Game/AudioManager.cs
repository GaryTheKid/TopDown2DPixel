using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // singleton
    public static AudioManager audioManager;

    // soundtracks
    public List<AudioClip> soundtracks;

    // player
    public AudioSource audioPlayer;

    // configs
    public float switchInterval;

    private int currentClipIndex;
    private bool isLooping;
    private IEnumerator _loopPlaying_Co;

    private void Awake()
    {
        audioManager = this;
    }

    private void Start()
    {
        LoadClip(0);
        LoopingSoundTracks();
    }

    public void LoopingSoundTracks()
    {
        if (_loopPlaying_Co == null)
        {
            isLooping = true;
            _loopPlaying_Co = Co_LoopPlaying();
            StartCoroutine(_loopPlaying_Co);
        }
    }
    IEnumerator Co_LoopPlaying()
    {
        while (isLooping)
        {
            // play
            Play();

            // wait till it finishes
            yield return new WaitUntil(() => audioPlayer.time >= audioPlayer.clip.length);
            audioPlayer.clip = null;

            // wait interval
            yield return new WaitForSecondsRealtime(switchInterval);

            // switch to the next
            LoadNextClip();
        }

        // clear
        _loopPlaying_Co = null;
    }

    public void Play()
    {
        audioPlayer.Play();
    }

    public void Pause()
    {
        isLooping = false;
        audioPlayer.Pause();
    }

    public void PlayOrPause()
    {
        if (audioPlayer.clip == null)
        {
            isLooping = false;
            return;
        }

        if (audioPlayer.isPlaying)
            Pause();
        else if (_loopPlaying_Co == null)
            LoopingSoundTracks();
        else
            Play();
    }

    private void LoadNextClip()
    {
        currentClipIndex++;
        if (currentClipIndex >= soundtracks.Count)
            currentClipIndex = 0;

        audioPlayer.clip = soundtracks[currentClipIndex];
    }

    private void LoadClip(int index)
    {
        currentClipIndex = index;
        audioPlayer.clip = soundtracks[currentClipIndex];
    }
}
