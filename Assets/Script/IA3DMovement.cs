using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IA3DMovement : MonoBehaviour
{
    public Transform Target;
    public float Speed;
    public float BoostSpeed;
    public Vector3 Velocity;
    public float MaxBoostTime;
    public float stopRadius;
    public float slowRadius;
    public float avoidDistance;
    
    private float CurrentBoostTime;
    private bool isBoosting;
    private bool asToSlow;
    private bool asToStop;
    private float distanceFromTarget;
    
    private void Update()
    {
        Velocity.Normalize();
        if (asToStop)
        {
            Velocity = Vector3.zero;
            asToStop = false;
        }
        if (asToSlow)
        {
            Velocity *= Speed * (distanceFromTarget / slowRadius);
            asToSlow = false;
        }
        else
            Velocity *= Speed;
        if (isBoosting)
        {
            Velocity *= BoostSpeed;
            CurrentBoostTime += Time.deltaTime;
            if (CurrentBoostTime >= MaxBoostTime)
            {
                CurrentBoostTime = 0;
                isBoosting = false;
            }
        }
        transform.position += Velocity * Time.deltaTime;
        Velocity = Vector3.zero;
    }

    public void Flee(Vector3 target)
    {
        Velocity += transform.position - target;
    }

    public void Evade(Transform target = null)
    {
        if (target == null)
            target = Target;
        var distance = Vector3.Distance(transform.position, target.position);
        var ahead = distance / 10;
        
        if (target.forward == transform.forward || Mathf.Approximately(target.forward.magnitude, 0))
            Flee(target.position);
        var futurePosition = target.position + target.forward * ahead;
        
        Flee(futurePosition);
    }

    public void Pursue(Transform target = null)
    {
        if (target == null)
            target = Target;
        var distance = Vector3.Distance(target.position, transform.position);
        var ahead = distance / 10;
        var futurePosition = target.position + target.forward * ahead;
        
        Seek(futurePosition);
    }
    
    public void Seek(Vector3 target)
    {
        Velocity += target - transform.position;
    }

    public void Arrive(Transform target = null)
    {
        if (target == null)
            target = Target;
        var desiredVelocity = target.position - transform.position;
        var distance = desiredVelocity.magnitude;

        if (distance <= stopRadius)
            asToStop = true;
        else if (distance < slowRadius)
        {
            asToSlow = true;
            distanceFromTarget = distance;
        }
        Velocity += desiredVelocity;
    }

    public void Avoid()
    {
        var currentObstacle = GetClosestObstacle();
        if (!currentObstacle)
            return;
        Evade(currentObstacle);
    }


    Transform GetClosestObstacle()
    {
        Transform currentObstacle = null;
        //Find the closest obstacle
        // make to overlap to find colliders edges and not colliders centers
        var obstacles = new Collider[5];
        var size = Physics.OverlapSphereNonAlloc(transform.position, avoidDistance, obstacles); 
        foreach (var obstacle in obstacles)
        {
            size--;
            if (size < 0)
                break;
            if (obstacle.GetInstanceID() == GetInstanceID() || obstacle.name == name)
                continue;
            if (!currentObstacle.IsUnityNull() && Vector3.Distance(obstacle.transform.position, transform.position)
            < Vector3.Distance(currentObstacle.position, transform.position))
                currentObstacle = obstacle.transform;
            else if (currentObstacle.IsUnityNull())
                currentObstacle = obstacle.transform;
        }
        return currentObstacle;
    }

    public void Boost()
    {
        if (isBoosting)
        {
            return;
        }
        isBoosting = true;
    }
}
