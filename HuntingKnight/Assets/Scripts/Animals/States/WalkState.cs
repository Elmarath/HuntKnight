using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class WalkState : State
{
    private bool isDestinationReachable;
    private bool isDestinationReached;

    public WalkState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.WALK);
        Debug.Log("Entered Walk State");

        // check if animal wants to walk to a specific point
        if (commonAnimal.walkToPosition != Vector3.zero)
        {
            isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, commonAnimal.walkToPosition);
            commonAnimal.walkToPosition = Vector3.zero;
        }
        else
        {
            // if not, walk to a random point
            isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, AnimalNavigationHelper.GetRandomWalkablePosition
            (commonAnimal.agent, commonAnimal.fieldOfView, Color.magenta));
        }
    }

    public override void Exit()
    {
        base.Exit();
        // When exiting set the animation variables
    }
    public override void HandleInput()
    {
        base.HandleInput();
        // Handle the input and set conditions for exiting the state
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isDestinationReachable)
        {
            commonAnimal.walkToPosition = AnimalNavigationHelper.GetRandomWalkablePosition(commonAnimal.agent, commonAnimal.fieldOfView.viewRadius, 360, Color.magenta);
            stateMachine.ChangeState(commonAnimal.idleState);
        }
        else if (AnimalNavigationHelper.IsCloseEnough(commonAnimal.agent.transform.position,
             commonAnimal.agent.destination, commonAnimal.agent.stoppingDistance))
        {
            stateMachine.ChangeState(commonAnimal.idleState);
        }

        // Check the conditions for exiting the state if true change the state
    }



}