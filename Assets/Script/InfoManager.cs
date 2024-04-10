using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour
{
    public enum Teams
    {
        Gryffindor,
        Slytherin,
        Hufflepuff,
        Ravenclaw
    }
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI classText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI midGameText;
    public GameObject ScreenDuringGame;
    public GameObject ScreenWinGryffindor;
    public GameObject ScreenWinSlytherin;
    public GameObject ScreenWinHufflepuff;
    public GameObject ScreenWinRavenclaw;
    public GameObject MidGameScreen;
    public SetIARole firstTeamIa;
    public SetIARole secondTeamIa;
    public Quaffle quaffle;
    public GameObject GryffindorFlags;
    public GameObject SlytherinFlags;
    public GameObject HufflepuffFlags;
    public GameObject RavenclawFlags;
    public Material secondHalfSkybox;
    public GameObject screenPause;

    public float scoreTeam1 = 0;
    public float scoreTeam2 = 0;
    public float gameLength = 0;
    public float totalTime = 480;
    private string classPlayer = "Seeker";
    public Teams teamFirst = Teams.Slytherin;
    public Teams teamSecond = Teams.Gryffindor;
    private bool gameEnded = false;
    private bool midGame = false;

    void Start()
    {
        timerText.text = "Time: " + gameLength;
        classPlayer = PlayerPrefs.GetString("PlayerClass");
        SetBothTeams();
        classText.text = "Class: " + classPlayer;
        scoreText.text = teamFirst + ": " + scoreTeam1 + " - " + scoreTeam2 + " : " + teamSecond;
    }

    private void SetBothTeams()
    {
        string team = PlayerPrefs.GetString("Team");
        switch (team)
        {
            case "Gryffindor":
                teamFirst = Teams.Gryffindor;
                teamSecond = Teams.Slytherin;
                GryffindorFlags.SetActive(true);
                SlytherinFlags.SetActive(true);
                break;
            case "Slytherin":
                teamFirst = Teams.Slytherin;
                teamSecond = Teams.Gryffindor;
                GryffindorFlags.SetActive(true);
                SlytherinFlags.SetActive(true);
                break;
            case "Hufflepuff":
                teamFirst = Teams.Hufflepuff;
                teamSecond = Teams.Ravenclaw;
                HufflepuffFlags.SetActive(true);
                RavenclawFlags.SetActive(true);
                break;
            case "Ravenclaw":
                teamFirst = Teams.Ravenclaw;
                teamSecond = Teams.Hufflepuff;
                HufflepuffFlags.SetActive(true);
                RavenclawFlags.SetActive(true);
                break;
        }

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ScreenDuringGame.SetActive(false);
            screenPause.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
        }
        if (gameEnded)
        {
            ScreenDuringGame.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            WinningTeam();
            return;
        }
        gameLength += Time.deltaTime;
        int minutes = Mathf.FloorToInt(gameLength / 60F);
        timerText.text = minutes.ToString() + "m " + (gameLength % 60).ToString("00") + "s";
        if (gameLength >= totalTime / 2 && !midGame) {
            ScreenDuringGame.SetActive(false);
            MidGameScreen.SetActive(true);
            Time.timeScale = 0;
            midGame = true;
            Cursor.lockState = CursorLockMode.None;
            midGameText.text = teamFirst.ToString() + " : " + scoreTeam1 + " - " + scoreTeam2 + " : " + teamSecond.ToString();
        }
        if (gameLength >= totalTime)
        {
            Debug.Log("Game ended !");
            timerText.text = "Game ended !";
            gameEnded = true;
        }
        scoreText.text = teamFirst.ToString() + ": " + scoreTeam1 + " - " + scoreTeam2 + " : " + teamSecond.ToString();
    }

    public void LaunchGameSecondHalf()
    {
        MidGameScreen.SetActive(false);
        ScreenDuringGame.SetActive(true);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        RenderSettings.skybox = secondHalfSkybox;
        firstTeamIa.LaunchGame();
        secondTeamIa.LaunchGame();
        quaffle.startGame();
    }

    public void ResumeGame()
    {
        ScreenDuringGame.SetActive(true);
        screenPause.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetGameEnded(Teams catchSnitchTeam)
    {
        if (catchSnitchTeam == teamFirst)
        {
            scoreTeam1 += 30;
        }
        else
        {
            scoreTeam2 += 30;
        }
        gameEnded = true;
    }

    void WinningTeam()
    {
        GameObject screen = null;
        if (scoreTeam1 > scoreTeam2) {
            switch (teamFirst)
            {
                case Teams.Gryffindor:
                    screen = ScreenWinGryffindor;
                    break;
                case Teams.Slytherin:
                    screen = ScreenWinSlytherin;
                    break;
                case Teams.Hufflepuff:
                    screen = ScreenWinHufflepuff;
                    break;
                case Teams.Ravenclaw:
                    screen = ScreenWinRavenclaw;
                    break;
            }
        } else {
            switch (teamSecond)
            {
                case Teams.Gryffindor:
                    screen = ScreenWinGryffindor;
                    break;
                case Teams.Slytherin:
                    screen = ScreenWinSlytherin;
                    break;
                case Teams.Hufflepuff:
                    screen = ScreenWinHufflepuff;
                    break;
                case Teams.Ravenclaw:
                    screen = ScreenWinRavenclaw;
                    break;
            }
        }
        if (screen == null) return;
        screen.SetActive(true);
        if (scoreTeam1 > scoreTeam2)
            screen.GetComponent<ScreenEndgame>().SetTeam(teamFirst, scoreTeam1);
        else
            screen.GetComponent<ScreenEndgame>().SetTeam(teamSecond, scoreTeam2);
    }
}
