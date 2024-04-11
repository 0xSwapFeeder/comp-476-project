using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAChaserAgent : IAAgent
{
    Quaffle currentQuaffle;
    public float catchingDistance = 1;
    public float throwingDistance = 10;
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
        currentQuaffle = GameObject.FindGameObjectWithTag("Quaffle").GetComponent<Quaffle>();
        movement.Target = currentQuaffle.transform;
        movement.Pursue();
        Debug.Log("Distance to quaffle: " + Vector3.Distance(transform.position, currentQuaffle.transform.position));
        if (Vector3.Distance(transform.position, currentQuaffle.transform.position) < catchingDistance && currentQuaffle.transform.parent == null)
        {
            Debug.Log("Picking up quaffle");
            currentQuaffle.PickUp(transform);
            hasQuaffle = true;
            movement.Target = oppositeGoal;
        }
    }
    
    Transform getOppositeGoal()
    {
        var goal1 = GameObject.FindGameObjectWithTag("GoalFirst");
        var goal2 = GameObject.FindGameObjectWithTag("GoalSecond");
        return Vector3.Distance(transform.position, goal1.transform.position) < Vector3.Distance(transform.position, goal2.transform.position) ? goal2.transform : goal1.transform;
    }
}
