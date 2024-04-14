using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SettingsMenu : MonoBehaviour
{
    AudioManager audioManager;

    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start() 
    {
        audioManager = FindObjectOfType<AudioManager>();
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void FullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }

    public void SetVolume(float volume) {
        audioManager.ChangeVolume(volume);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
}
