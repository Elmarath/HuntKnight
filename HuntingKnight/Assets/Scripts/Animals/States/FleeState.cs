using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class FleeState : State
{
    // Condition variables set here
    private bool _isGettingChased;
    private bool _isDestinationReachable;
    private int _recentlyChased = 0;
    private Transform _target;

    public FleeState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.FLEE);
        commonAnimal.fieldOfView.viewAngle = 330f; // if it sees it would look at it so larger angle given
        commonAnimal.isFleeing = true;
        commonAnimal.agent.speed = commonAnimal.animalAttributes.runSpeed;
        commonAnimal.agent.acceleration = 16;
        commonAnimal.agent.autoBraking = false;

        if (commonAnimal.visibleRunFromThese.Count > 0)
        {
            _recentlyChased = 3;
            _target = AnimalNavigationHelper.GetClosestGameObject(commonAnimal.agent, commonAnimal.visibleRunFromThese.ToArray()).transform;
            Vector3 dir = (commonAnimal.transform.position - _target.position).normalized;
            _isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, AnimalNavigationHelper.GetEscapePosition(
                commonAnimal.agent, dir));

            Debug.DrawRay(commonAnimal.transform.position, dir, Color.red, 5f);
            _isGettingChased = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _isGettingChased = false;
        _target = null;
        commonAnimal.isFleeing = false;
        commonAnimal.fieldOfView.viewAngle = commonAnimal.animalAttributes.sightAngle;
        commonAnimal.agent.speed = commonAnimal.animalAttributes.walkSpeed;
        commonAnimal.agent.acceleration = 8;
        // When exiting set the animation variables
    }
    public override void HandleInput()
    {
        base.HandleInput();

        // is chasing animal still in radius
        if (_isGettingChased)
        {
            if (AnimalNavigationHelper.IsCloseEnough(commonAnimal.agent.transform.position, commonAnimal.agent.destination, commonAnimal.agent.stoppingDistance + 1))
            {
                if (AnimalNavigationHelper.IsCloseEnough(commonAnimal.transform.position, _target.position, commonAnimal.animalAttributes.sightRange))
                {
                    _recentlyChased = 3;
                    Debug.Log("RecentlyChased: " + _recentlyChased);
                }
                if (_recentlyChased > 0)
                {
                    if (_recentlyChased == 1)
                    {
                        // last run
                        commonAnimal.animations.PlayAnimation(commonAnimal.animations.FLEE);
                        commonAnimal.agent.speed = commonAnimal.animalAttributes.walkSpeed + commonAnimal.animalAttributes.runSpeed / 2;
                        commonAnimal.agent.acceleration = 8;
                        commonAnimal.agent.autoBraking = true;
                    }
                    Vector3 dir = (commonAnimal.transform.position - _target.position).normalized;
                    _isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, AnimalNavigationHelper.GetEscapePosition(
    commonAnimal.agent, dir));
                    _recentlyChased--;
                    Debug.DrawRay(commonAnimal.transform.position, dir, Color.blue, 5f);
                }
            }
            if (_recentlyChased <= 0)
            {
                _isGettingChased = false;
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!_isDestinationReachable)
        {
            stateMachine.ChangeState(commonAnimal.idleState);
        }
        else if (AnimalNavigationHelper.IsCloseEnough(commonAnimal.agent.transform.position,
     commonAnimal.agent.destination, commonAnimal.agent.stoppingDistance + 1) && !_isGettingChased)
        {
            commonAnimal.agent.velocity *= 0.5f;
            stateMachine.ChangeState(commonAnimal.idleState);
        }

        // Check the conditions for exiting the state if true change the state
    }

}