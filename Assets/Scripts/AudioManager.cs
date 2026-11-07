
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;

    public AudioClip overworldMusic;
    public AudioClip caveMusic;

    public AudioClip[] variousSFX;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        musicSource.clip = overworldMusic;
        musicSource.Play();
    }

    public void PlayMusicSFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlayRandomSFX(params AudioClip[] clips)
    {
        if (clips.Length == 0) return;

        variousSFX = clips;

        int index = Random.Range(0, variousSFX.Length);
        sfxSource.PlayOneShot(variousSFX[index]);
    }
}
