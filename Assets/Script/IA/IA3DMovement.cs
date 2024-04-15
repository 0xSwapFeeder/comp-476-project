using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
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
    private float stunDuration = 3;
    private float stunTime;
    private bool isStunned = false;
    
    private float minYPosition = -12;
    private float maxYPosition = 18;
    
    private float minXPosition = -50;
    private float maxXPosition = 40;
    
    private float minZPosition = -18;
    private float maxZPosition = 24;

    private Vector3 center = new (-4, -6, 8);

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
        if (isStunned)
        {
            stunTime += Time.deltaTime;
            if (stunTime >= stunDuration)
            {
                isStunned = false;
                stunTime = 0;
            }
            Velocity = Vector3.zero;
            currentVelocity = Vector3.zero;
            // Make the player rotate on himself
            transform.Rotate(Vector3.up, 100 * Time.deltaTime);
            return;
        }
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
        
        transform.rotation = Quaternion.Euler(newRotation);
    }

    private void getNeighbours()
    {
        Collider[] aiColliders = Physics.OverlapSphere(transform.position, 10);
        int[] sameTeamCount = new int[4];
        int[] enemyTeamCount = new int[4];

        foreach (Collider collider in aiColliders) {
            IAAgent player = collider.GetComponent<IAAgent>();
            if (player != null && player != this) {
                if (player.team == gameObject.GetComponent<IAAgent>().team)
                    sameTeamCount[(int)player.type]++;
                else
                    enemyTeamCount[(int)player.type]++;
            }
        }
        float speedBoost = 1;

        for (int i = 0; i < 4; i++) {
            if (sameTeamCount[i] > 0) {
                switch ((IAAgent.AgentType)i) {
                    case IAAgent.AgentType.Keeper:
                        speedBoost += sameTeamCount[i] * 0.06f;
                        break;
                    case IAAgent.AgentType.Seeker:
                        speedBoost += 0.1f;
                        break;
                    case IAAgent.AgentType.Chaser:
                        speedBoost += sameTeamCount[i] * 0.06f;
                        break;
                    case IAAgent.AgentType.Beater:
                        speedBoost += 0.1f;
                        break;
                }
            }
            if (enemyTeamCount[i] > 0) {
                switch ((IAAgent.AgentType)i) {
                    case IAAgent.AgentType.Keeper:
                        speedBoost += sameTeamCount[i] * 0.06f;
                        break;
                    case IAAgent.AgentType.Seeker:
                        speedBoost += 0.1f;
                        break;
                    case IAAgent.AgentType.Chaser:
                        speedBoost -= sameTeamCount[i] * 0.06f;
                        break;
                    case IAAgent.AgentType.Beater:
                        speedBoost += 0.06f;
                        break;
                }
            }
        }


        Velocity *= speedBoost;
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
        // if behind terrain limits go to the center
        if (transform.position.y < minYPosition || transform.position.y > maxYPosition
            || transform.position.x < minXPosition || transform.position.x > maxXPosition
            || transform.position.z < minZPosition || transform.position.z > maxZPosition)
        {
            Seek(center);
        }
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
            if (!obstacle.gameObject.CompareTag("Wall"))
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

    public void StayAtDistance(float safeDistance, Transform target = null)
    {
        if (target == null) 
            target = Target;
        // Calculate the desired position to stay at a safe distance from the target
        Vector3 desiredPosition = target.position + (transform.position - target.position).normalized * safeDistance;
        // Calculate the desired velocity to reach the desired position
        Vector3 desiredVelocity = (desiredPosition - transform.position).normalized * Speed;
        // Adjust the current velocity towards the desired velocity
        Velocity = Vector3.Lerp(Velocity, desiredVelocity, Time.deltaTime);
    }

    public void GetToTarget(Transform target)
    {
        Vector3 desiredVelocity = (target.position - transform.position).normalized * Speed;
        Velocity = Vector3.Lerp(Velocity, desiredVelocity, Time.deltaTime);
    }

    void Stun()
    {
        if (isStunned)
            return;
        isStunned = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        // stun the player if it hits a wall or another player if the player is slower than the other player
        if (collision.gameObject.CompareTag("Wall"))
            Stun();
        var otherAgent = collision.gameObject.GetComponent<IAAgent>();
        if (otherAgent != null)
        {
            var otherSpeed = otherAgent.GetComponent<IA3DMovement>().currentVelocity.magnitude;
            if ( otherSpeed > currentVelocity.magnitude)
                Stun();
            Debug.Log("Collision: " + collision.gameObject.tag);
        }
        
    }
}
