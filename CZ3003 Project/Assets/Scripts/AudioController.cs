// Authors: Daryl Neo
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip levelMusic;
    public 
    // Start is called before the first frame update
    void Start()
    {
        // Plays levelMusic while fading away existing soundtrack
        if(levelMusic!=null)
        AudioManager.Instance.PlayMusicWithFade(levelMusic);
    }

}
