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
    private bool hasQuaffle;
    public float pickupCooldown = 1f;
    private float lastThrowTime = -1f;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        string goalTag = teamPlayer == TeamsTag.Team1 ? "GoalSecond" : "GoalFirst";
        goal = GetGoal(goalTag);
        oppositeGoal = GetGoal(goalTag == "GoalFirst" ? "GoalSecond" : "GoalFirst");
    }

    void Update()
    {
        base.Update();
        setClosestQuaffle();
        var quaffleHolder = quaffle.transform.parent;
        if (quaffleHolder != null)
        {
            var isPlayer = quaffleHolder.GetComponentInParent<Player>();
                InfoManager.Teams quaffleHolderTeam = InfoManager.Teams.Hufflepuff;
                if (isPlayer != null) {
                    quaffleHolderTeam = isPlayer.team;
                } else {
                    var isIAAgent = quaffleHolder.GetComponent<IAAgent>();
                    if (isIAAgent != null)
                        quaffleHolderTeam = quaffleHolder.GetComponent<IAAgent>().team;
                }
            if (quaffleHolder != null && quaffleHolder != gameObject.transform)
            {
                if (quaffleHolderTeam != team && Vector3.Distance(transform.position, quaffleHolder.transform.position) < defenseDistance)
                {
                    movement.Target = quaffleHolder.transform;
                    movement.GetToTarget(quaffleHolder.transform);
                }
            }
        }
        if (Vector3.Distance(transform.position, quaffle.transform.position) < catchingDistance && quaffle.transform.parent == null && Time.time - lastThrowTime >= pickupCooldown)
        {
            quaffle.PickUp(transform);
            hasQuaffle = true;
            movement.Target = oppositeGoal;
        }
        if (hasQuaffle)
        {
            transform.LookAt(oppositeGoal);
            quaffle.GetThrown(transform);
            hasQuaffle = false;
            lastThrowTime = Time.time;
        }
    }
    
    Transform GetGoal(string Tag)
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag(Tag);
        Transform closestGoal = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject goal in goals)
        {
            float distance = Vector3.Distance(transform.position, goal.transform.position);
            if (distance < closestDistance)
            {
                closestGoal = goal.transform;
                closestDistance = distance;
            }
        }
        return closestGoal;
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
