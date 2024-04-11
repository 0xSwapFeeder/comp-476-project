using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1);
        ChangeVolume();
        
    }

    void Update()
    {
        
    }

    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        audioManager.ChangeVolume(volumeSlider.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
    }
}
