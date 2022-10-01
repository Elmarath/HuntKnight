using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class ChaseState : State
{
    // Condition variables set here
    private bool _isChasing;
    private bool _isDestinationReachable;
    private bool _readyToAttack;
    private int _recentlyChased = 0;
    private Transform _target;

    public ChaseState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("ChaseState");
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.CHASE);
        commonAnimal.fieldOfView.viewAngle = 330f; // if it sees it would look at it so larger angle given
        commonAnimal.isChasing = true;
        commonAnimal.agent.speed = commonAnimal.animalAttributes.runSpeed;
        commonAnimal.agent.acceleration = 16;
        commonAnimal.agent.autoBraking = false;
        _readyToAttack = false;
        _target = null;
        Collider[] targetsInAttackRadius = Physics.OverlapSphere(commonAnimal.agent.transform.position, commonAnimal.animalAttributes.sightRange, commonAnimal.animalAttributes.attackThese);
        Collider targetCollider = AnimalNavigationHelper.GetClosestCollider(commonAnimal.agent, targetsInAttackRadius);
        if (targetCollider != null)
        {
            _target = targetCollider.transform;
            _isChasing = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _isChasing = false;
        _target = null;
        commonAnimal.isChasing = false;
        commonAnimal.fieldOfView.viewAngle = commonAnimal.animalAttributes.sightAngle;
        commonAnimal.agent.speed = commonAnimal.animalAttributes.walkSpeed;
        commonAnimal.agent.acceleration = 8;
        // When exiting set the animation variables
    }

    public override void HandleInput()
    {
        base.HandleInput();

        // is chasing animal still in radius
        if (_isChasing)
        {
            _isDestinationReachable = AnimalNavigationHelper.GoDestination(commonAnimal.agent, _target.position);

            // attack
            Debug.Log("Distance: " + Vector3.Distance(commonAnimal.transform.position, _target.position));
            if (AnimalNavigationHelper.IsCloseEnough(commonAnimal.transform.position, _target.position, commonAnimal.animalAttributes.attackRange))
            {
                _readyToAttack = true;
            }
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_readyToAttack)
        {
            stateMachine.ChangeState(commonAnimal.attackState);
        }
        else if (!_isDestinationReachable || _target == null)
        {
            stateMachine.ChangeState(commonAnimal.idleState);
        }
    }

}