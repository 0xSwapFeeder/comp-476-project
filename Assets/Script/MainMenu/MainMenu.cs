using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        // Load the next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void FullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen;
    }

    public void setKeeper() {
        PlayerPrefs.SetString("PlayerClass", "Keeper");
        PlayerPrefs.Save();
    }

    public void setChaser() {
        PlayerPrefs.SetString("PlayerClass", "Chaser");
        PlayerPrefs.Save();
    }

    public void setSeeker() {
        PlayerPrefs.SetString("PlayerClass", "Seeker");
        PlayerPrefs.Save();
    }

    public void setBeater() {
        PlayerPrefs.SetString("PlayerClass", "Beater");
        PlayerPrefs.Save();
    }
}
