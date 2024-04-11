using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAKeeperAgent : IAAgent
{
    public float distanceToGoal = 20;
    public float defenseDistance = 30;
    public float catchingDistance = 3;

    
    private Quaffle quaffle;
    private Transform goal;
    private Transform oppositeGoal;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        var goal1 = GameObject.FindGameObjectWithTag("GoalFirst");
        var goal2 = GameObject.FindGameObjectWithTag("GoalSecond");
        goal = Vector3.Distance(transform.position, goal1.transform.position) < Vector3.Distance(transform.position, goal2.transform.position) ? goal1.transform : goal2.transform;
        oppositeGoal = goal == goal1.transform ? goal2.transform : goal1.transform;
    }

    void Update()
    {
        base.Update();
        setClosestQuaffle();
        if (quaffle != null && Vector3.Distance(transform.position, quaffle.transform.position) < defenseDistance)
        {
            transform.LookAt(quaffle.transform);
            movement.Target = quaffle.transform;
            movement.Pursue();
        }
        else if (Vector3.Distance(transform.position, goal.position) > distanceToGoal)
        {
            transform.LookAt(goal);
            movement.Target = goal;
            movement.Arrive();
        }
        else if (quaffle != null && Vector3.Distance(transform.position, quaffle.transform.position) < catchingDistance)
        {
            quaffle.PickUp(transform);
            quaffle.GetThrown(oppositeGoal);
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
                this.quaffle = quaffle.GetComponent<Quaffle>();
            }
        }
    }
}
