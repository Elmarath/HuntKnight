using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class RunState : State
{
    // Condition variables set here
    private bool _isGettingChased;
    private bool _isChasing;
    private bool _isDestinationReachable;
    private int _recentlyChased = 0;
    private bool _readyToAttack = false;
    private Transform _target;

    public RunState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        _readyToAttack = false;
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.RUN);
        commonAnimal.fieldOfView.viewAngle = 330f; // if it sees it would look at it so larger angle given
        commonAnimal.isRunning = true;
        commonAnimal.agent.speed = commonAnimal.animalAttributes.runSpeed;
        commonAnimal.agent.acceleration = 16;
        commonAnimal.agent.autoBraking = false;
        // decide whether to flee or chase
        if (commonAnimal.visibleRunFromThese.Count > 0)
        {
            _recentlyChased = 3;
            _target = commonAnimal.visibleRunFromThese[0].transform;
            Vector3 dir = (commonAnimal.transform.position - _target.position).normalized;
            _isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, AnimalNavigationHelper.GetRandomWalkablePosition(
        commonAnimal.agent, dir, commonAnimal.fieldOfView.viewRadius, commonAnimal.fieldOfView.viewAngle - 30f, true, Color.magenta));
            _isGettingChased = true;
            _isChasing = false;
        }
        else if (commonAnimal.visibleAttackThese.Count > 0)
        {
            _target = commonAnimal.visibleAttackThese[0].transform;
            _isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, _target.position);
            _isGettingChased = false;
            _isChasing = true;
        }
        // When entered set the animation variables (generally use GetComponent<AnimalKind>().variableName)
        // When entered set conditions for exiting the state
    }

    public override void Exit()
    {
        base.Exit();
        _isGettingChased = true;
        _isChasing = false;
        _target = null;
        commonAnimal.isRunning = false;
        commonAnimal.fieldOfView.viewAngle = commonAnimal.animalAttributes.sightAngle;
        commonAnimal.agent.speed = commonAnimal.animalAttributes.walkSpeed;
        commonAnimal.agent.stoppingDistance = 0.5f;
        commonAnimal.agent.acceleration = 8;
        // When exiting set the animation variables
    }
    public override void HandleInput()
    {
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
                        commonAnimal.agent.speed = commonAnimal.animalAttributes.walkSpeed + commonAnimal.animalAttributes.runSpeed / 3;
                        commonAnimal.agent.stoppingDistance = 3f;
                        commonAnimal.agent.acceleration = 8;
                        commonAnimal.agent.autoBraking = true;
                    }
                    Vector3 dir = (commonAnimal.transform.position - _target.position).normalized;
                    _isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, AnimalNavigationHelper.GetRandomWalkablePosition(
                commonAnimal.agent, dir, commonAnimal.fieldOfView.viewRadius, commonAnimal.fieldOfView.viewAngle - 30f, true, Color.magenta));
                    _recentlyChased--;
                }
            }
            if (_recentlyChased <= 0)
            {
                _isGettingChased = false;
            }
        }

        else if (_isChasing)
        {
            if (AnimalNavigationHelper.IsCloseEnough(commonAnimal.agent.transform.position, _target.position, commonAnimal.animalAttributes.sightRange))
            {
                _isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, _target.position);
                if (AnimalNavigationHelper.IsCloseEnough(commonAnimal.agent.transform.position, _target.position, commonAnimal.animalAttributes.attackRange))
                {
                    _readyToAttack = true;
                }
            }
            else
            {
                _isChasing = false;
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

        else if (_readyToAttack)
        {
            Debug.Log("Ready to attack");
            stateMachine.ChangeState(commonAnimal.attackState);
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