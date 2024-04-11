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
        Thrown,
    }
    public InfoManager.Teams thrownBy;
    public BludgerState State;
    public Transform Target;
    public IA3DMovement movement;
    
    void Start()
    {
        GameObject particleSystem = transform.GetChild(0).gameObject;
        string playerClass = PlayerPrefs.GetString("PlayerClass");
        if (playerClass == "Beater") {
            particleSystem.SetActive(true);
        } else {
            particleSystem.SetActive(false);
        }
        

    }
    
    public void Throw(Transform target, float force, InfoManager.Teams team)
    {
        Target = target;
        movement.Speed = force;
        thrownBy = team;
        State = BludgerState.Thrown;
    }
    
    public void Grab()
    {
        Target = null;
        State = BludgerState.Grabed;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (State == BludgerState.Thrown)
        {
            movement.Target = Target;
            movement.Pursue();
            movement.Boost();
            movement.Avoid();
        }
        else if (State == BludgerState.Chased)
        {
            foreach (var iaBeaterAgent in GetAllBeaters())
            {
                movement.Target = iaBeaterAgent.transform;
                movement.Evade();
            }
            movement.Avoid();
        }
    }
    
    List<IABeaterAgent> GetAllBeaters()
    {
        return FindObjectsOfType<IABeaterAgent>().ToList();
    }
}
