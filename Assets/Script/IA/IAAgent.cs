using System.Collections;
using System.Collections.Generic;
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
}
