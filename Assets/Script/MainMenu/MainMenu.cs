using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI InfoText;
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
        InfoText.text = "When playing as a Keeper, you have to protect your goal from the Chasers. You can also catch the Quaffle and throw it to your teammates.";
    }

    public void setChaser() {
        PlayerPrefs.SetString("PlayerClass", "Chaser");
        PlayerPrefs.Save();
        InfoText.text = "When playing as a Chaser, you have to score goals by throwing the Quaffle through the hoops. You can also pass the Quaffle to your teammates.";
    }

    public void setSeeker() {
        PlayerPrefs.SetString("PlayerClass", "Seeker");
        PlayerPrefs.Save();
        InfoText.text = "When playing as a Seeker, you have to catch the Golden Snitch to end the game.";
    }

    public void setBeater() {
        PlayerPrefs.SetString("PlayerClass", "Beater");
        PlayerPrefs.Save();
        InfoText.text = "When playing as a Beater, you have to protect your teammates from the Bludgers by hitting them with your bat.";
    }

    public void BackToMainMenu() {
        SceneManager.LoadScene(0);
    }

    public void SetGryffindor() {
        PlayerPrefs.SetString("Team", "Gryffindor");
        PlayerPrefs.Save();
    }

    public void SetSlytherin() {
        PlayerPrefs.SetString("Team", "Slytherin");
        PlayerPrefs.Save();
    }

    public void SetHufflepuff() {
        PlayerPrefs.SetString("Team", "Hufflepuff");
        PlayerPrefs.Save();
    }

    public void SetRavenclaw() {
        PlayerPrefs.SetString("Team", "Ravenclaw");
        PlayerPrefs.Save();
    }

}
