using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IABeaterAgent : IAAgent
{
    Bludger currentBludger;

    public float throwingDistance;
    public float throwingForce;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBludger == null) {
            
        }
    }

    bool hasToDefendTeamFromIncomingBludger()
    {
        return false;
    }
    
    List<Bludger> getBludgers()
    {
        return GameObject.FindGameObjectsWithTag("Bludger").Select(g => g.GetComponent<Bludger>()).ToList();
    }
    
}
