using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class Run : State
{
    // Condition variables set here
    private Rabbit rabbit; // state owner
    private GameObject closestPredator;

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
        GameObject closestPredator = null;
        if (isTherePredeator())
        {
            closestPredator = FindClosestPredator();
        }

        // Handle the input and set conditions for exiting the state
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // turn its back to predator
        if (closestPredator != null)
        {
            Vector3 direction = closestPredator.transform.position - animal.transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            animal.transform.rotation = Quaternion.Slerp(animal.transform.rotation, rotation, rabbit.logicUpdateInterval * 0.5f);
        }
        // run from predator
        if (closestPredator != null)
        {
            rabbit.GotoDestination(closestPredator.transform.position);
        }

        if (closestPredator == null)
        {
            stateMachine.ChangeState(rabbit.idle);
        }
    }

    public GameObject FindClosestPredator()
    {
        // Find the closest predator
        GameObject closestPredeator = null;

        for (int i = 0; i < rabbit.visibleRunFroms.Count; i++)
        {
            if (closestPredeator == null)
            {
                closestPredeator = rabbit.visibleRunFroms[i];
            }
            else
            {
                if (Vector3.Distance(rabbit.transform.position, rabbit.visibleRunFroms[i].transform.position) < Vector3.Distance(rabbit.transform.position, closestPredeator.transform.position))
                {
                    closestPredeator = rabbit.visibleRunFroms[i];
                }
            }
        }
        return closestPredeator;
    }

    public bool isTherePredeator()
    {
        // Check if there is a predator
        return rabbit.visibleRunFroms.Count > 0;
    }

}