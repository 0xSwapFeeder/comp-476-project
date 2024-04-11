using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAChaserAgent : IAAgent
{
    Quaffle currentQuaffle;
    public float catchingDistance = 2;
    public float throwingDistance = 50;
    private bool hasQuaffle;
    private Transform oppositeGoal;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        oppositeGoal = getOppositeGoal();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (hasQuaffle)
        {
            movement.Arrive();
            if (Vector3.Distance(transform.position, oppositeGoal.position) <= throwingDistance)
            {
                currentQuaffle.GetThrown(oppositeGoal);
                hasQuaffle = false;
            }
            return;
        }
        setClosestQuaffle();
        movement.Target = currentQuaffle.transform;
        movement.Pursue();
        if (Vector3.Distance(transform.position, currentQuaffle.transform.position) < catchingDistance && currentQuaffle.transform.parent == null)
        {
            Debug.Log("Picking up quaffle");
            currentQuaffle.PickUp(transform);
            hasQuaffle = true;
            movement.Target = oppositeGoal;
        }
    }
    
    void setClosestQuaffle()
    {
        GameObject[] quaffles = GameObject.FindGameObjectsWithTag("Quaffle");
        float minDistance = float.MaxValue;
        foreach (var quaffle in quaffles)
        {
            float distance = Vector3.Distance(transform.position, quaffle.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                currentQuaffle = quaffle.GetComponent<Quaffle>();
            }
        }
    }
    Transform getOppositeGoal()
    {
        var goal1 = GameObject.FindGameObjectWithTag("GoalFirst");
        var goal2 = GameObject.FindGameObjectWithTag("GoalSecond");
        return Vector3.Distance(transform.position, goal1.transform.position) < Vector3.Distance(transform.position, goal2.transform.position) ? goal2.transform : goal1.transform;
    }
}
