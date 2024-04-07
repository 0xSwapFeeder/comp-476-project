using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bludger : MonoBehaviour
{
    public enum BludgerState
    {
        Grabed,
        Chased,
        ThrownByRed,
        ThrownByBlue
    }
    public BludgerState State;
    public Transform Target;
    public IA3DMovement movement;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void Throw(Transform target, float force)
    {
        Target = target;
        movement.Speed = force;
    }
    
    public void Grab()
    {
        Target = null;
        State = BludgerState.Grabed;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (State == BludgerState.ThrownByBlue || State == BludgerState.ThrownByRed)
        {
            movement.Target = Target;
            movement.Pursue();
            movement.Boost();
        }
        else if (State == BludgerState.Chased)
        {
            foreach (var iaBeaterAgent in GetAllBeaters())
            {
                movement.Target = iaBeaterAgent.transform;
                movement.Evade();
            }
        }
    }
    
    List<IABeaterAgent> GetAllBeaters()
    {
        return FindObjectsOfType<IABeaterAgent>().ToList();
    }
}
