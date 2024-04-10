using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IAAgent : MonoBehaviour
{
    public enum AgentType
    {
        Keeper,
        Seeker,
        Chaser,
        Beater
    }
    
    public IA3DMovement movement;
    public InfoManager.Teams team;
    public AgentType type;


    protected void Start()
    {
        movement = GetComponent<IA3DMovement>();
    }

    // Update is called once per frame
    protected void Update()
    {
        movement.Avoid();
    }

    public void FlockWithSameTeamMemberType()
    {
        var sameTeamMembers = GetSameTeamTypes();
        var averagePosition = sameTeamMembers.Aggregate(Vector3.zero, (current, member) => current + member.transform.position) / sameTeamMembers.Count;
        var averageVelocity = sameTeamMembers.Aggregate(Vector3.zero, (current, member) => current + member.movement.Velocity) / sameTeamMembers.Count;
        var direction = averagePosition - transform.position;
        movement.Velocity += direction.normalized;
    }

    public List<IAAgent> GetSameTeamTypes()
    {
        var teamMembers = GetTeamMembers();
        return teamMembers.Where(t => t.type == type).ToList();
    }
    
    public List<IAAgent> GetTeamMembers()
    {
        var chasers = GameObject.FindGameObjectsWithTag("Chaser").Select(g => g.GetComponent<IAAgent>()).Where(g => g.team == team).ToList();
        var seekers = GameObject.FindGameObjectsWithTag("Seeker").Select(g => g.GetComponent<IAAgent>()).Where(g => g.team == team).ToList();
        var keepers = GameObject.FindGameObjectsWithTag("Keeper").Select(g => g.GetComponent<IAAgent>()).Where(g => g.team == team).ToList();
        var beaters = GameObject.FindGameObjectsWithTag("Beater").Select(g => g.GetComponent<IAAgent>()).Where(g => g.team == team).ToList();
        var teamMembers = new List<IAAgent>();
        teamMembers.AddRange(chasers);
        teamMembers.AddRange(seekers);
        teamMembers.AddRange(keepers);
        teamMembers.AddRange(beaters);
        return teamMembers;
        
    }
    
    public List<IAAgent> GetOppositeTeamMembers()
    {
        var chasers = GameObject.FindGameObjectsWithTag("Chaser").Select(g => g.GetComponent<IAAgent>()).Where(g => g.team != team).ToList();
        var seekers = GameObject.FindGameObjectsWithTag("Seeker").Select(g => g.GetComponent<IAAgent>()).Where(g => g.team != team).ToList();
        var keepers = GameObject.FindGameObjectsWithTag("Keeper").Select(g => g.GetComponent<IAAgent>()).Where(g => g.team != team).ToList();
        var beaters = GameObject.FindGameObjectsWithTag("Beater").Select(g => g.GetComponent<IAAgent>()).Where(g => g.team != team).ToList();
        var teamMembers = new List<IAAgent>();
        teamMembers.AddRange(chasers);
        teamMembers.AddRange(seekers);
        teamMembers.AddRange(keepers);
        teamMembers.AddRange(beaters);
        return teamMembers;
    }
}
