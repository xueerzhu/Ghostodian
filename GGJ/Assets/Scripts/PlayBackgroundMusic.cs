using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackgroundMusic : MonoBehaviour
{
    public AudioClip musicToPlay;
    private AudioSource musicAudioSource;

    private static PlayBackgroundMusic instance = null;

    public static PlayBackgroundMusic Instance
     {
         get
         {
             if (instance == null)
             {
                 instance = (PlayBackgroundMusic)FindObjectOfType(typeof(PlayBackgroundMusic));
             }
             return instance;
         }
     }

    void Awake() {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Play() {
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.Play();
    }

}


