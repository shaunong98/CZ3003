using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    private bool firstMusicSourceisPlaying;
    private static AudioManager instance;
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
        DontDestroyOnLoad(this.gameObject);
        musicSource=this.gameObject.AddComponent<AudioSource>();
        musicSource2=this.gameObject.AddComponent<AudioSource>();
        sfxSource=this.gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource2.loop = true;
    }

    public void PlayMusic(AudioClip musicClip)
    {
       AudioSource activeSource = (firstMusicSourceisPlaying) ? musicSource : musicSource2;
       musicSource.clip = musicClip;
       activeSource.volume = 1;
       musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayMusicWithFade(AudioClip newClip, float transitionTime=1.0f)
    {
        AudioSource activeSource = (firstMusicSourceisPlaying) ? musicSource : musicSource2;

        StartCoroutine(UpdateMusicWithFade(activeSource,newClip,transitionTime));
    }

    public void PlayMusicWithCrossFade(AudioClip musicClip, float transitionTime=1.0f)
    {
        AudioSource activeSource = (firstMusicSourceisPlaying) ? musicSource : musicSource2;
        AudioSource newSource = (firstMusicSourceisPlaying) ? musicSource2 : musicSource;

        firstMusicSourceisPlaying = !firstMusicSourceisPlaying;

        newSource.clip = musicClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource,newSource,transitionTime));
    }

    private IEnumerator UpdateMusicWithCrossFade(AudioSource original,AudioSource newSource,float transitionTime)
    {
        float t= 0.0f;
        for(t=0.0f; t<=transitionTime; t+= Time.deltaTime)
        {
            original.volume=(1-(t/transitionTime));
            newSource.volume= (t/transitionTime);
            yield return null;
        }
        original.Stop();
    }
    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        if(!activeSource.isPlaying)
        {
            activeSource.Play();
        }

        float t = 0.0f;

        //fade out
        for(t = 0; t< transitionTime;t+=Time.deltaTime)
        {
            activeSource.volume = (1-(t/transitionTime));
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        //fade in
        for(t = 0; t< transitionTime;t+=Time.deltaTime)
        {
            activeSource.volume = (t/transitionTime);
            yield return null;
        }
    }

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
