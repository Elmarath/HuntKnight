using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class WalkState : State
{
    private bool _isDestinationReachable;
    private bool _isGoingToASpecificPosition;
    private bool _isGoingForNutrient;

    public WalkState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.WALK);

        // check if animal wants to walk to a specific point
        if (commonAnimal.walkToPosition != Vector3.zero)
        {
            _isGoingToASpecificPosition = true;
            _isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, commonAnimal.walkToPosition);
            commonAnimal.walkToPosition = Vector3.zero;
        }
        else
        {
            // if not, walk to a random point
            _isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, AnimalNavigationHelper.GetRandomWalkablePosition
            (commonAnimal.agent, commonAnimal.fieldOfView, Color.magenta));
        }

        if (!_isDestinationReachable)
        {
            _isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, AnimalNavigationHelper.GetRandomWalkablePosition
            (commonAnimal.agent, commonAnimal.fieldOfView.viewRadius, 360, Color.magenta));
        }
    }

    public override void Exit()
    {
        base.Exit();
        _isGoingToASpecificPosition = false;
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

        if (!_isDestinationReachable)
        {
            stateMachine.ChangeState(commonAnimal.idleState);
        }
        // if going to a specific position, check if reached
        if (_isGoingToASpecificPosition)
        {
            // was it going for nutrient?
            if (commonAnimal.isGoingForNutrient && AnimalNavigationHelper.IsCloseEnough(commonAnimal.agent.transform.position,
 commonAnimal.agent.destination, commonAnimal.agent.stoppingDistance))
            {
                commonAnimal.isGoingForNutrient = false;
                stateMachine.ChangeState(commonAnimal.eatState);
            }
            // else reach anyway
            else if (AnimalNavigationHelper.IsCloseEnough(commonAnimal.agent.transform.position,
commonAnimal.agent.destination, commonAnimal.agent.stoppingDistance))
            {
                commonAnimal.isGoingForNutrient = false;
                stateMachine.ChangeState(commonAnimal.eatState);
            }
        }


        else if (AnimalNavigationHelper.IsCloseEnough(commonAnimal.agent.transform.position,
             commonAnimal.agent.destination, commonAnimal.agent.stoppingDistance))
        {
            stateMachine.ChangeState(commonAnimal.idleState);
        }

        // Check the conditions for exiting the state if true change the state
    }



}