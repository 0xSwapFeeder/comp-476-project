using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static InfoManager;

public class ScreenEndgame : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private Teams teamFirst;

    public void SetTeam(Teams team, float score)
    {
        teamFirst = team;
        scoreText.text = teamFirst.ToString() + " wins with " + score + " points!";
    }
}
