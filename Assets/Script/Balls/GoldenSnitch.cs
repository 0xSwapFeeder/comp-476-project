using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static InfoManager;

public class GoldenSnitch : MonoBehaviour
{
    public InfoManager infoManager;
    public IA3DMovement movement;
    public float boostRadius;

    void Start()
    {
        movement = GetComponent<IA3DMovement>();
        GameObject particleSystem = transform.GetChild(0).gameObject;
        string playerClass = PlayerPrefs.GetString("PlayerClass");
        if (playerClass == "Seeker") {
            particleSystem.SetActive(true);
        } else {
            particleSystem.SetActive(false);
        }
    }

    void Update()
    {
        var closestChaser = getClosestChaser();
        movement.Target = closestChaser;
        movement.Evade();
        if (Vector3.Distance(transform.position, closestChaser.position) < boostRadius)
            movement.Boost();
        movement.Avoid();
    }

    Transform getClosestChaser()
    {
        GameObject[] chasers = GameObject.FindGameObjectsWithTag("Chaser");
        float minDistance = float.MaxValue;
        Transform closestChaser = null;
        foreach (var chaser in chasers)
        {
            float distance = Vector3.Distance(transform.position, chaser.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestChaser = chaser.transform;
            }
        }
        return closestChaser;
    }

    public void GetCatch(Teams team)
    {
        infoManager.SetGameEnded(team);
    }
}
