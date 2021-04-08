using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioLogin : MonoBehaviour
{
    [SerializeField] private AudioClip levelMusic;
    public 
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(levelMusic);
    }

}
