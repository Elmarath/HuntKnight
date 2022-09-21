using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This is an example of a base state. It is not used in the project.
public class Run : State
{
    // Condition variables set here
    private Rabbit rabbit; // state owner
    private GameObject predatorToRunFrom;
    private Vector3 runDestination;
    private bool isArrived = false;

    public Run(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Run State Entered");
        rabbit = animal.GetComponent<Rabbit>();
        rabbit.agent.speed = rabbit.runningSpeed;
        rabbit.currentState = "Run";
        predatorToRunFrom = FindClosestPredator();
        runDestination = CreateRandomRunAwayDestination(animal.viewRadius * 1.50f, animal.viewAngle / 2, animal.minSearchDistance * 2);
        animal.GotoDestination(runDestination);
        // Set Animation Variables
        rabbit.goRun = true;
    }

    public override void Exit()
    {
        base.Exit();
        rabbit.agent.speed = rabbit.walkingSpeed;
        // When exiting set the animation variables
        rabbit.goRun = false;
    }
    public override void HandleInput()
    {
        base.HandleInput();
        isArrived = animal.IsCloseEnough(runDestination, animal.closeEnoughTolerance);
        Debug.Log("isArrived: " + isArrived);
        Debug.Log("animal.closeEnoughTolerance: " + animal.closeEnoughTolerance);
        Debug.Log("Distance to destination: " + Vector3.Distance(animal.transform.position, runDestination));

        // Handle the input and set conditions for exiting the state
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // run from predator
        if (isArrived)
        {
            if (!HasRunnedAwayFromPredator())
            {
                runDestination = CreateRandomRunAwayDestination(animal.viewRadius * 1.50f, animal.viewAngle / 2, animal.minSearchDistance * 2);
                animal.GotoDestination(runDestination);
            }
            else
            {
                stateMachine.ChangeState(rabbit.idle);
            }
        }
    }

    public GameObject FindClosestPredator()
    {
        GameObject closestPredator = null;

        for (int i = 0; i < rabbit.visibleRunFroms.Count; i++)
        {
            if (closestPredator == null)
            {
                closestPredator = rabbit.visibleRunFroms[i];
            }
            else
            {
                if (Vector3.Distance(animal.transform.position, rabbit.visibleRunFroms[i].transform.position) < Vector3.Distance(animal.transform.position, closestPredator.transform.position))
                {
                    closestPredator = rabbit.visibleRunFroms[i];
                }
            }
        }
        return closestPredator;

    }

    public bool HasRunnedAwayFromPredator()
    {
        // Check if the predator is not in the viewRadius
        if (predatorToRunFrom != null)
        {
            float distance = Vector3.Distance(animal.transform.position, predatorToRunFrom.transform.position);
            if (distance > rabbit.viewRadius)
            {
                return true;
            }
        }
        return false;
    }

    public override void HandleInterrupts()
    {
        base.HandleInterrupts();
        // Handle the interrupts and set conditions for exiting the state
        if (rabbit.hasSeenPredator && !(rabbit.stateMachine.CurrentState == rabbit.run))
        {
            stateMachine.ChangeState(rabbit.run);
        }
    }

    public Vector3 CreateRandomRunAwayDestination(float viewRadius, float viewAngle, float minSearchDistance)
    {
        Vector3 randomDirection;
        Vector2 randomDirectionV2;
        Vector3 finalPosition;
        NavMeshHit hit;

        for (int i = 0; i < 10; i++)
        {
            randomDirectionV2 = Random.insideUnitCircle * viewRadius;
            randomDirection = new Vector3(randomDirectionV2.x, 0, randomDirectionV2.y);
            randomDirection += rabbit.transform.position;

            float distance = Vector3.Distance(randomDirection, rabbit.transform.position);

            // Distance Check
            if (distance < minSearchDistance)
            {
                continue;
            }
            // Angle Check
            if (Vector3.Angle(rabbit.transform.forward, randomDirection - rabbit.transform.position) < viewAngle * 2)
            {
                continue;
            }
            // Navigation Validation Check
            bool isWalkable = NavMesh.SamplePosition(randomDirection, out hit, rabbit.closeEnoughTolerance, NavMesh.AllAreas);
            if (!isWalkable)
            {
                continue;
            }

            finalPosition = randomDirection;

            if (rabbit.isIndicatorsWanted)
            {
                rabbit.SpawnIndicator(rabbit.movementIndicator, finalPosition);
            }

            return finalPosition;
        }

        finalPosition = rabbit.transform.position - (rabbit.transform.forward * minSearchDistance);
        return finalPosition;
    }



}