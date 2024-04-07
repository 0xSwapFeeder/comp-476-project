using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIARole : MonoBehaviour
{
    private string playerClass;
    public GameObject bat;
    public GameObject headProtection;
    public GameObject seekerGoggles;
    public Transform Player = null;
    public Transform SeekerPosition;
    public Transform[] BeaterPositions;
    public Transform KeeperPosition;
    public Transform[] ChaserPosition;
    private int keeper = 1;
    private int seeker = 1;
    private int beater = 2;
    private int chaser = 3;
    // Start is called before the first frame update
    void Start()
    {
        var BeatersPosition = BeaterPositions;
        var ChasersPositions = ChaserPosition;
        if (Player != null)
        {
            playerClass = PlayerPrefs.GetString("PlayerClass");
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
            switch(playerClass)
            {
                case "Seeker":
                    GiveGoggles(Player.transform.GetChild(0));
                    Player.transform.position = SeekerPosition.position;
                    break;
                case "Beater" :
                    GiveBat(Player.transform.GetChild(0));
                    Player.transform.position = BeatersPosition[0].position;                    
                    BeatersPosition = BeatersPosition[1..];
                    break;
                case "Keeper":
                    Player.transform.position = KeeperPosition.position;
                    GiveHeadProtection(Player.transform.GetChild(0));
                    break;
                case "Chaser":
                    Player.transform.position = ChasersPositions[0].position;
                    ChasersPositions = ChasersPositions[1..];
                    break;
            }
        }
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player")) continue;
            if (keeper > 0) {
                child.tag = "Keeper";
                GiveHeadProtection(child.transform.GetChild(0));
                child.position = KeeperPosition.position;
                keeper -= 1;
            }
            else if (seeker > 0) {
                child.tag = "Seeker";
                GiveGoggles(child.transform.GetChild(0));
                child.position = SeekerPosition.position;
                seeker -= 1;
            }
            else if (beater > 0) {
                child.tag = "Beater";
                GiveBat(child.transform.GetChild(0));
                child.position = BeaterPositions[0].position;
                BeaterPositions = BeaterPositions[1..];
                beater -= 1;
            }
            else if (chaser > 0) {
                child.tag = "Chaser";
                child.position = ChaserPosition[0].position;
                ChaserPosition = ChaserPosition[1..];
                chaser -= 1;
            }
        }
        
    }

    void GiveBat(Transform child)
    {
        GameObject batInstance = Instantiate(bat, child);
        batInstance.transform.localPosition = new Vector3(0.053f, 0.936f, 0.616f);
        batInstance.transform.localRotation = Quaternion.Euler(0, -80.767f, 40.923f);
        batInstance.transform.parent = child;
    }

    void GiveHeadProtection(Transform child)
    {
        GameObject headProtectionInstance = Instantiate(headProtection, child);
        headProtectionInstance.transform.localPosition = new Vector3(-0.014f, 1.203f, 0.168f);
        headProtectionInstance.transform.localRotation = Quaternion.Euler(0, 90, 0);
        headProtectionInstance.transform.parent = child;
    }

    void GiveGoggles(Transform child)
    {
        GameObject gogglesInstance = Instantiate(seekerGoggles, child);
        gogglesInstance.transform.localPosition = new Vector3(-0.00300000003f,-0.681999981f,-0.275000006f);
        gogglesInstance.transform.localRotation = Quaternion.Euler(-76.766f, 0, 0);
        gogglesInstance.transform.parent = child;
    }

    public void LaunchGame()
    {
        foreach(Transform child in transform)
        {
            if (child.CompareTag("Keeper")) {
                child.position = KeeperPosition.position;
            } else if (child.CompareTag("Seeker")) {
                child.position = SeekerPosition.position;
            } else if (child.CompareTag("Beater")) {
                child.position = BeaterPositions[0].position;
                BeaterPositions = BeaterPositions[1..];
            } else if (child.CompareTag("Chaser")) {
                child.position = ChaserPosition[0].position;
                ChaserPosition = ChaserPosition[1..];
            }
        }
    }
}
