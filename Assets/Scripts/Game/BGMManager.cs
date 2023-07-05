using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    // Singleton instance
    public static BGMManager singleton;

    public List<AudioClip> daySoundtracks;
    public List<AudioClip> nightSoundtracks;

    private AudioSource audioPlayer;

    private void Awake()
    {
        // Ensure only one instance of AudioManager exists
        if (singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Get the AudioSource component
        audioPlayer = GetComponent<AudioSource>();

        // Play the initial soundtrack
        PlayDaytimeSoundtrack();
    }

    // Play a random daytime soundtrack
    public void PlayDaytimeSoundtrack()
    {
        if (daySoundtracks.Count == 0)
        {
            Debug.LogWarning("No daytime soundtracks available.");
            return;
        }

        // Select a random daytime soundtrack from the list
        AudioClip soundtrack = daySoundtracks[Random.Range(0, daySoundtracks.Count)];

        // Check if the same soundtrack is already playing
        if (audioPlayer.clip == soundtrack && audioPlayer.isPlaying)
        {
            audioPlayer.loop = true; // Enable looping
            return;
        }

        // Play the new soundtrack
        audioPlayer.loop = true; // Enable looping
        PlaySoundtrack(soundtrack);
    }

    // Play a random nighttime soundtrack
    public void PlayNighttimeSoundtrack()
    {
        if (nightSoundtracks.Count == 0)
        {
            Debug.LogWarning("No nighttime soundtracks available.");
            return;
        }

        // Select a random nighttime soundtrack from the list
        AudioClip soundtrack = nightSoundtracks[Random.Range(0, nightSoundtracks.Count)];

        // Check if the same soundtrack is already playing
        if (audioPlayer.clip == soundtrack && audioPlayer.isPlaying)
        {
            audioPlayer.loop = true; // Enable looping
            return;
        }

        // Play the new soundtrack
        audioPlayer.loop = true; // Enable looping
        PlaySoundtrack(soundtrack);
    }

    // Play a specific soundtrack
    private void PlaySoundtrack(AudioClip soundtrack)
    {
        StartCoroutine(TransitionSoundtracks(soundtrack));
    }

    // Coroutine for the soundtrack transition
    private IEnumerator TransitionSoundtracks(AudioClip newSoundtrack)
    {
        const float transitionDuration = 1.0f; // Adjust the duration of the transition as needed

        // Fade out the current soundtrack
        float startVolume = audioPlayer.volume;
        float startTime = Time.time;
        while (Time.time < startTime + transitionDuration)
        {
            float elapsed = Time.time - startTime;
            float t = elapsed / transitionDuration;

            audioPlayer.volume = Mathf.Lerp(startVolume, 0f, t);

            yield return null;
        }

        audioPlayer.Stop();

        // Play the new soundtrack
        audioPlayer.clip = newSoundtrack;
        audioPlayer.Play();

        // Fade in the new soundtrack
        startTime = Time.time;
        while (Time.time < startTime + transitionDuration)
        {
            float elapsed = Time.time - startTime;
            float t = elapsed / transitionDuration;

            audioPlayer.volume = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        // Ensure the volume is set to 1 at the end of the transition
        audioPlayer.volume = 1f;
    }
}