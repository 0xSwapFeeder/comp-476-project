using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI classText;
    public float gameLength = 480;
    private string classPlayer = "Seeker";
    void Start()
    {
        timerText.text = "Time: " + gameLength;
        classPlayer = PlayerPrefs.GetString("PlayerClass");
        classText.text = "Class: " + classPlayer;
    }


    void Update()
    {
        gameLength -= Time.deltaTime;
        timerText.text = "Time: " + Mathf.Round(gameLength);
        if (gameLength <= 0)
        {
            timerText.text = "Game ended !";
        }   
    }
}
