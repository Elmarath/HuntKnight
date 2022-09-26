using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class WalkState : State
{
    private CommonAnimal _commonAnimal;
    private StateMachine _stateMachine;
    private bool isDestinationReachable;
    private bool isDestinationReached;

    public WalkState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
        _commonAnimal = commonAnimal;
        _stateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Walk State");
        _commonAnimal.isWalking = true;

        // check if animal wants to walk to a specific point
        if (_commonAnimal.walkToPosition != Vector3.zero)
        {
            isDestinationReachable = AnimalNavigationHelper.GoDestination(_commonAnimal.agent, _commonAnimal.walkToPosition);
        }
        else
        {
            // if not, walk to a random point
            isDestinationReachable = AnimalNavigationHelper.GoDestination(_commonAnimal.agent, AnimalNavigationHelper.GetRandomWalkablePosition
            (commonAnimal.agent, commonAnimal.animalAttributes.sightRange, commonAnimal.animalAttributes.sightAngle, Color.magenta));
        }
    }

    public override void Exit()
    {
        base.Exit();
        _commonAnimal.isWalking = false;
        // When exiting set the animation variables
    }
    public override void HandleInput()
    {
        base.HandleInput();
        Debug.Log(Vector3.Distance(_commonAnimal.agent.transform.position, _commonAnimal.agent.destination));
        // Handle the input and set conditions for exiting the state
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        Debug.Log(AnimalNavigationHelper.IsCloseEnough(_commonAnimal.agent.transform.position, _commonAnimal.agent.destination, _commonAnimal.agent.stoppingDistance));

        if (!isDestinationReachable)
        {
            _stateMachine.ChangeState(commonAnimal.idleState);
        }
        else if (AnimalNavigationHelper.IsCloseEnough(_commonAnimal.agent.transform.position,
             _commonAnimal.agent.destination, _commonAnimal.agent.stoppingDistance - 0.2f))
        {
            _stateMachine.ChangeState(commonAnimal.idleState);
        }

        // Check the conditions for exiting the state if true change the state
    }



}