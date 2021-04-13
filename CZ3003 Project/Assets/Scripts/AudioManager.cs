// Authors: Daryl Neo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    public float musicVolume;
    private bool firstMusicSourceisPlaying;
    private static AudioManager instance;

    // Sets Current Audio Manager as instance, else create a new Audio Manager
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if(instance == null)
                {
                    instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private void Awake() 
    {
        // Set gameobject as  DontDestroyOnLoad
        DontDestroyOnLoad(this.gameObject);

        // Create multiple Audio Source
        musicSource=this.gameObject.AddComponent<AudioSource>();
        musicSource2=this.gameObject.AddComponent<AudioSource>();
        sfxSource=this.gameObject.AddComponent<AudioSource>();

        // Enables looping of audio
        musicSource.loop = true;
        musicSource2.loop = true;
    }

    // Plays a specific music set by the parameter 
    public void PlayMusic(AudioClip musicClip)
    {
        // Set first Audio Source as active Audio Source
        AudioSource activeSource = musicSource;

        musicSource.clip = musicClip;

        // Adjust Volume to 1 of active Audio Source
        activeSource.volume = 1;

        musicSource.Play();
    }

    // Plays a specific SFX set by the parameter 
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // Plays a specific SFX set by the parameter with volume overload
    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    // Allows a new music to be played while fading the previous music
    public void PlayMusicWithFade(AudioClip newClip, float transitionTime=1.0f)
    {
        AudioSource activeSource = musicSource;

        // Adjust Volume to current volume of active Audio Source
        musicVolume = activeSource.volume;

        // Starts Coroutine to initialize the audio swap
        StartCoroutine(UpdateMusicWithFade(activeSource,newClip,transitionTime));
    }

    // Audio Swap with existing music
    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        // If no music is playing, play music of activeSource
        if(!activeSource.isPlaying)
        {
            activeSource.Play();
        }

        // Set counter to 0
        float t = 0.0f;

        // Music fades out over time
        for(t = 0; t< transitionTime;t+=Time.deltaTime)
        {
            activeSource.volume = (musicVolume - ((t/ transitionTime) * musicVolume));
            yield return null;
        }

        // Stop current music
        activeSource.Stop();

        // Swap out current music to new music
        activeSource.clip = newClip;

        // Play new current music
        activeSource.Play();

        // Music fades in over time
        for(t = 0; t< transitionTime;t+=Time.deltaTime)
        {
            activeSource.volume = (t / transitionTime) * musicVolume;
            yield return null;
        }
    }

    // Pauses current AudioSource
    public void PauseMusic()
    {
        musicSource.Pause();
        musicSource2.Pause();
    }

    // Resumes current AudioSource
    public void ResumeMusic()
    {
        musicSource.UnPause();
        musicSource2.UnPause();
    }

    // Volume Control
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        musicSource2.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume=volume;
    }
}
