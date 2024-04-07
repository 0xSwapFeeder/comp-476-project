using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI classText;
    public TextMeshProUGUI scoreText;
    public float scoreTeam1 = 0;
    public float scoreTeam2 = 0;
    public float gameLength = 480;
    private string classPlayer = "Seeker";
    void Start()
    {
        timerText.text = "Time: " + gameLength;
        classPlayer = PlayerPrefs.GetString("PlayerClass");
        classText.text = "Class: " + classPlayer;
        scoreText.text = "Score: " + scoreTeam1 + " - " + scoreTeam2;
    }


    void Update()
    {
        gameLength -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Round(gameLength);
        if (gameLength <= 0)
        {
            timerText.text = "Game ended !";
        }
        scoreText.text = "Score: " + scoreTeam1 + " - " + scoreTeam2;
    }
}
