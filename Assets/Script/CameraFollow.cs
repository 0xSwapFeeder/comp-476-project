using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Settings")]
    public Transform target;
    public float pLerp = .02f;
    public float rLerp = .01f;
    [Header("UI Elements")]
    public GameObject arrowLeft;
    public GameObject arrowRight;
    public GameObject arrowUp;
    public GameObject arrowDown;
    [Header("Balls")]
    public GameObject quaffle;
    public GameObject snitch;
    public GameObject bludgerOne;
    public GameObject bludgerTwo;
    private string playerClassPref;


    void Start()
    {
        playerClassPref = PlayerPrefs.GetString("PlayerClass");
    }

    void Update()
    {
        transform.SetPositionAndRotation(Vector3.Lerp(transform.position, target.position, pLerp), Quaternion.Lerp(transform.rotation, target.rotation, rLerp));
        FollowQuaffle();
        FollowSnitch();
        FollowBludgers();
    }

    void FollowQuaffle()
    {
        if (playerClassPref == "Chaser" || playerClassPref == "Keeper")
        {
            Vector3 ballScreenPos = Camera.main.WorldToScreenPoint(quaffle.transform.position);
            if (ballScreenPos.x < 0)
            {
                arrowLeft.SetActive(true);
                arrowRight.SetActive(false);
            }
            else if (ballScreenPos.x > Screen.width)
            {
                arrowLeft.SetActive(false);
                arrowRight.SetActive(true);
            }
            else
            {
                arrowLeft.SetActive(false);
                arrowRight.SetActive(false);
            }
            if (ballScreenPos.y < 0)
            {
                arrowDown.SetActive(true);
                arrowUp.SetActive(false);
            }
            else if (ballScreenPos.y > Screen.height)
            {
                arrowDown.SetActive(false);
                arrowUp.SetActive(true);
            }
            else
            {
                arrowDown.SetActive(false);
                arrowUp.SetActive(false);
            }
        }
    }

    void FollowSnitch()
    {
        if (playerClassPref == "Seeker")
        {
            Vector3 snitchScreenPos = Camera.main.WorldToScreenPoint(snitch.transform.position);
            if (snitchScreenPos.x < 0)
            {
                arrowLeft.SetActive(true);
                arrowRight.SetActive(false);
            }
            else if (snitchScreenPos.x > Screen.width)
            {
                arrowLeft.SetActive(false);
                arrowRight.SetActive(true);
            }
            else
            {
                arrowLeft.SetActive(false);
                arrowRight.SetActive(false);
            }
            if (snitchScreenPos.y < 0)
            {
                arrowDown.SetActive(true);
                arrowUp.SetActive(false);
            }
            else if (snitchScreenPos.y > Screen.height)
            {
                arrowDown.SetActive(false);
                arrowUp.SetActive(true);
            }
            else
            {
                arrowDown.SetActive(false);
                arrowUp.SetActive(false);
            }
        }
    }

    void FollowBludgers()
    {
        if (playerClassPref == "Beater")
        {
            Vector3 bludgerOneScreenPos = Camera.main.WorldToScreenPoint(bludgerOne.transform.position);
            Vector3 bludgerTwoScreenPos = Camera.main.WorldToScreenPoint(bludgerTwo.transform.position);
            if (bludgerOneScreenPos.x < 0 || bludgerTwoScreenPos.x < 0)
            {
                arrowLeft.SetActive(true);
                arrowRight.SetActive(false);
            }
            else if (bludgerOneScreenPos.x > Screen.width || bludgerTwoScreenPos.x > Screen.width)
            {
                arrowLeft.SetActive(false);
                arrowRight.SetActive(true);
            }
            else
            {
                arrowLeft.SetActive(false);
                arrowRight.SetActive(false);
            }
            if (bludgerOneScreenPos.y < 0 || bludgerTwoScreenPos.y < 0)
            {
                arrowDown.SetActive(true);
                arrowUp.SetActive(false);
            }
            else if (bludgerOneScreenPos.y > Screen.height || bludgerTwoScreenPos.y > Screen.height)
            {
                arrowDown.SetActive(false);
                arrowUp.SetActive(true);
            }
            else
            {
                arrowDown.SetActive(false);
                arrowUp.SetActive(false);
            }
        }
    }
    
}
