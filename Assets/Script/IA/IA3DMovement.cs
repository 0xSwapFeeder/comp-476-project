using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class IA3DMovement : MonoBehaviour
{
    public Transform Target;
    public float Speed;
    public float BoostSpeed;
    public Vector3 Velocity;
    public float MaxBoostTime;
    public float BoostCooldown;
    public float stopRadius;
    public float slowRadius;
    public float avoidDistance;
    public float DirectionChangingTime;

    private Vector3 currentVelocity;
    private float currentDirectionChangingTime;
    private float CurrentBoostTime;
    private float CurrentBoostCooldown;
    private bool boostingOnCooldown;
    private bool isBoosting;
    private bool asToSlow;
    private bool asToStop;
    private float distanceFromTarget;
    private bool isChangingTarget;

    private void CheckSuddenDirectionChange()
    {
        // if the previous velocity (currentVelocity) and the Velocity are approximately opposite
        if (Vector3.Angle(currentVelocity, Velocity) > 90
            || currentVelocity == -Velocity
            || (Mathf.Approximately(currentVelocity.magnitude, 0) && !Mathf.Approximately(Velocity.magnitude, 0))
            || (Mathf.Approximately(Velocity.magnitude, 0) && !Mathf.Approximately(currentVelocity.magnitude, 0)))
        {
            isChangingTarget = true;
            currentDirectionChangingTime = 0;
        } 
    }
    
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
        UpdateBoost();
        getNeighbours();
        transform.position += Velocity * Time.deltaTime;
        CheckSuddenDirectionChange();
        ChangingDirection();
        Vector3 newRotation = transform.rotation.eulerAngles;
        newRotation.x = 0;

        transform.rotation = UnityEngine.Quaternion.Euler(newRotation);
    }

    private void getNeighbours()
    {
        Collider[] aiColliders = Physics.OverlapSphere(transform.position, 10);
        int sameTeamCount = 0;

        foreach (Collider collider in aiColliders)
        {
            IAAgent aiMovement = collider.GetComponent<IAAgent>();
            if (aiMovement != null && aiMovement != this)
                if (aiMovement.gameObject.tag == gameObject.tag)
                    sameTeamCount++;
        }

        Velocity *= 1f + (sameTeamCount * 0.03f);
    }

    private void ChangingDirection()
    {
        if (!isChangingTarget)
            currentVelocity = Velocity;
        else if (currentDirectionChangingTime >= DirectionChangingTime)
        {
            currentDirectionChangingTime = 0;
            isChangingTarget = false;
        }
        else
        {
            currentDirectionChangingTime += Time.deltaTime;
            transform.position += currentVelocity
            * Mathf.Lerp(1, 0,currentDirectionChangingTime / DirectionChangingTime)
            * Time.deltaTime;
        }
    }

    private void UpdateBoost()
    {
        if (isBoosting)
        {
            Velocity *= BoostSpeed;
            CurrentBoostTime += Time.deltaTime;
            if (CurrentBoostTime >= MaxBoostTime)
            {
                CurrentBoostTime = 0;
                isBoosting = false;
                boostingOnCooldown = true;
            }
        }

        if (!boostingOnCooldown) return;
        CurrentBoostCooldown += Time.deltaTime;
        if (!(CurrentBoostCooldown >= BoostCooldown)) return;
        CurrentBoostCooldown = 0;
        boostingOnCooldown = false;
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
            // if (gameObject.tag == "Chasser" && obstacle.CompareTag("Quaffle"))
            //     Debug.Log("ijdijsd" + "  " + gameObject.tag);
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
        if (isBoosting || boostingOnCooldown)
            return;
        isBoosting = true;
    }
}
