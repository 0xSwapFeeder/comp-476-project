using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IAChaserAgent : IAAgent
{
    Quaffle currentQuaffle;
    public float catchingDistance = 1;
    public float throwingDistance = 10;
    public float pickupCooldown = 2f;
    private float lastThrowTime = -1f;
    public bool hasQuaffle;
    private Transform oppositeGoal;
    List<IAAgent> teamChasers;
    List<IAAgent> enemyChasers;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        oppositeGoal = getOppositeGoal();
        teamChasers = GetSameTeamTypes();
        enemyChasers = GetOppositeTeamMembers().Where(t => t.type == AgentType.Chaser && t.team != team).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        currentQuaffle = GameObject.FindGameObjectWithTag("Quaffle").GetComponent<Quaffle>();
        movement.Target = currentQuaffle.transform;
        var quaffleHolder = currentQuaffle.transform.parent;

        if (hasQuaffle) {
            var enemyChaser = FindClosestEnemyChaser();
            if (enemyChaser != null)
            {
                if (IsInFront(enemyChaser.transform))
                {
                    var closestFriendChaser = FindClosestFriendChaser();
                    if (closestFriendChaser != null)
                    {
                        currentQuaffle.GetThrownAtPosition(closestFriendChaser.transform.position);
                        hasQuaffle = false;

                    } else {
                        movement.Avoid();
                    }
                } else if (IsCloseEnoughOfTheGoal())
                {
                    transform.LookAt(oppositeGoal);
                    currentQuaffle.GetThrown(transform);
                    hasQuaffle = false;
                    lastThrowTime = Time.time;
                } else {
                    movement.Target = getOppositeGoal();
                    movement.Pursue();
                }
            } else if (IsCloseEnoughOfTheGoal())
            {
                transform.LookAt(oppositeGoal);
                currentQuaffle.GetThrown(transform);
                hasQuaffle = false;
                lastThrowTime = Time.time;
            } else {
                movement.Target = getOppositeGoal();
                movement.Pursue();
            }
        } else if (quaffleHolder != null && quaffleHolder != gameObject.transform)
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

            if (quaffleHolderTeam == team)
            {
                movement.Target = quaffleHolder.transform;
                movement.StayAtDistance(5f);
            } else
            {
                movement.Target = quaffleHolder.transform;
                movement.GetToTarget(quaffleHolder.transform);
            }
        } else {
            movement.Target = currentQuaffle.transform;
            movement.Pursue();
        }
        if (Vector3.Distance(transform.position, currentQuaffle.transform.position) < catchingDistance && currentQuaffle.transform.parent == null && Time.time - lastThrowTime >= pickupCooldown)
        {
            currentQuaffle.PickUp(transform);
            hasQuaffle = true;
            movement.Target = oppositeGoal;
        }
    }
    
    Transform getOppositeGoal()
    {
        string goalTag = teamPlayer == TeamsTag.Team1 ? "GoalFirst" : "GoalSecond";
        GameObject[] goals = GameObject.FindGameObjectsWithTag(goalTag);
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

    bool IsCloseEnoughOfTheGoal()
    {
        return Vector3.Distance(transform.position, oppositeGoal.position) <= 15;
    }

    Transform FindClosestEnemyChaser()
    {
        if (enemyChasers == null) 
        {
            return null;
        }
        var closestEnemyChaser = enemyChasers.OrderBy(chaser => Vector3.Distance(transform.position, chaser.transform.position)).FirstOrDefault();
        return closestEnemyChaser != null ? closestEnemyChaser.transform : null;
    }

    bool IsInFront(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        return Mathf.Abs(angle) < 90;
    }

    Transform FindClosestFriendChaser()
    {
        var closestFriendChaser = teamChasers.OrderBy(chaser => Vector3.Distance(transform.position, chaser.transform.position)).FirstOrDefault();
        return closestFriendChaser != null ? closestFriendChaser.transform : null;
    }

    public void OnCollisionEnter(Collision collision) 
    {
        if (collision.transform.CompareTag("Chaser") && hasQuaffle) {
            currentQuaffle.GetThrown(gameObject.transform);
            hasQuaffle = false;
            movement.Target = currentQuaffle.transform;
            currentQuaffle.LoseBall();
        }
    }
}
