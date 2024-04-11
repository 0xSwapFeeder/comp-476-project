using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;
    public AudioClip background;
    public AudioClip goal;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void ChangeVolume(float volume)
    {
        musicSource.volume = volume;
        sfxSource.volume = volume;
    }

    public float GetCurrentVolume()
    {
        return musicSource.volume;
    }
}
