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
    public enum Team
    {
        Red,
        Blue
    }
    
    public IA3DMovement movement;
    public Team team;
    public AgentType type;

    // Update is called once per frame
    void Update()
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
        return team == Team.Red 
            ? GameObject.FindGameObjectsWithTag("RedTeam").Select(g => g.GetComponent<IAAgent>()).ToList()
            : GameObject.FindGameObjectsWithTag("BlueTeam").Select(g => g.GetComponent<IAAgent>()).ToList();
    }
}
