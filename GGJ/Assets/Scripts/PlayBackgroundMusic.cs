using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackgroundMusic : MonoBehaviour
{
    public AudioClip musicToPlay;
    private AudioSource musicAudioSource;


    public static PlayBackgroundMusic Music = null;


    void Awake()
    {
        if (Music == null) {
            Music = this;
        }
        

        DontDestroyOnLoad(gameObject);

        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.Play();
    }
}
