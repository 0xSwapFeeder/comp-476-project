using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIARole : MonoBehaviour
{
    private string playerClass;
    private int keeper = 1;
    private int seeker = 1;
    private int beater = 2;
    private int chaser = 3;
    // Start is called before the first frame update
    void Start()
    {
        playerClass = PlayerPrefs.GetString("PlayerClass");
        // Set every child except the one with the player class to be either seeker, beater, chaser or keeper according to the player class
        switch (playerClass)
        {
            case "Seeker":
                seeker -= 1;
                break;
            case "Beater":
                beater -= 1;
                break;
            case "Chaser":
                chaser -= 1;
                break;
            case "Keeper":
                keeper -= 1;
                break;
        }
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player")) continue;
            if (keeper > 0) {
                child.tag = "Keeper";
                keeper -= 1;
            }
            else if (seeker > 0) {
                child.tag = "Seeker";
                seeker -= 1;
            }
            else if (beater > 0) {
                child.tag = "Beater";
                beater -= 1;
            }
            else if (chaser > 0) {
                child.tag = "Chaser";
                chaser -= 1;
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
