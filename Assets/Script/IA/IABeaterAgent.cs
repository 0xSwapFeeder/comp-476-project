using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IABeaterAgent : IAAgent
{
    Bludger currentBludger;
    
    public float catchingDistance;
    public float throwingDistance;
    public float throwingForce;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBludger != null)
            ThrowBludger();
        else if (!hasToDefendTeamFromIncomingBludger())
            DefendTeamFromIncomingBludger();
        else
            ChaseBludger();

    }

    void ChaseBludger()
    {
        var bludgers = getBludgers().Where(b => b.State == Bludger.BludgerState.Chased);
        var closestBludger = bludgers.Aggregate((current, next) => Vector3.Distance(transform.position, current.transform.position) < Vector3.Distance(transform.position, next.transform.position) ? current : next);
        
        movement.Target = closestBludger.transform;
        movement.Pursue();
        
        if (Vector3.Distance(transform.position, closestBludger.transform.position) < catchingDistance)
        {
            currentBludger = closestBludger;
            currentBludger.Grab();
        }
    }
    
    void ThrowBludger()
    {
        var opponents = GetOppositeTeamMembers();
        var closestOpponent = opponents.Aggregate((current, next) => Vector3.Distance(transform.position, current.transform.position) < Vector3.Distance(transform.position, next.transform.position) ? current : next);
        
        movement.Target = closestOpponent.transform;
        movement.Pursue();
        
        if (Vector3.Distance(transform.position, closestOpponent.transform.position) < throwingDistance)
        {
            currentBludger.Throw(closestOpponent.transform, throwingForce, team);
            currentBludger = null;
        }
    }
    
    void DefendTeamFromIncomingBludger()
    {
        var bludgers = getBludgers().Where( b => b.State == Bludger.BludgerState.Thrown && b.thrownBy != team).ToList();
        var closestBludger = bludgers.Aggregate((current, next) => Vector3.Distance(transform.position, current.transform.position) < Vector3.Distance(transform.position, next.transform.position) ? current : next);
        
        movement.Target = closestBludger.transform;
        movement.Pursue();
    }

    bool hasToDefendTeamFromIncomingBludger()
    {
        var bludgers = getBludgers();

        return bludgers.Any(b => b.State == Bludger.BludgerState.Thrown && b.thrownBy != team) && currentBludger == null;
    }
    
    List<Bludger> getBludgers()
    {
        return GameObject.FindGameObjectsWithTag("Bludger").ToList().Select(g => g.GetComponent<Bludger>()).ToList();
    }
    
}
