using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class AttackState : State
{
    // Condition variables set here
    private Transform _target;

    private float _timeWhenEnteredState;
    private bool _isWaitTimeOver;
    private bool _isTargetStillInRadius;

    public AttackState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("AttackState");
        /////
        commonAnimal.stateLock = true;
        /////
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.ATTACK);
        _timeWhenEnteredState = Time.time;
        _isWaitTimeOver = false;

        Collider[] targetsInAttackRadius = Physics.OverlapSphere(commonAnimal.agent.transform.position, commonAnimal.animalAttributes.attackRange, commonAnimal.animalAttributes.attackThese);
        Collider targetCollider = AnimalNavigationHelper.GetClosestCollider(commonAnimal.agent, targetsInAttackRadius);
        if (targetCollider != null)
        {
            _target = targetCollider.transform;

            // communication with target
            if (_target.GetComponent<NavigationTestScript>() != null)
            {
                _target.GetComponent<NavigationTestScript>().isBeingAttacked = true;
                //_target.GetComponent<CommonAnimal>().isTakingDamage = true;
            }

            commonAnimal.agent.isStopped = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        /////
        commonAnimal.stateLock = false;
        /////
        commonAnimal.agent.isStopped = true;
        commonAnimal.agent.isStopped = false;
        commonAnimal.isAttacking = false;
        _isTargetStillInRadius = false;
        // When exiting set the animation variables
    }
    public override void HandleInput()
    {
        base.HandleInput();
        _isWaitTimeOver = (Time.time - _timeWhenEnteredState) >= commonAnimal.animalAttributes.attackSpeed;
        _isTargetStillInRadius = AnimalNavigationHelper.IsCloseEnough(commonAnimal.agent.transform.position, _target.position, commonAnimal.animalAttributes.sightRange);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isWaitTimeOver && _isTargetStillInRadius)
        {
            stateMachine.ChangeState(commonAnimal.chaseState);
        }
        else if (_isWaitTimeOver && !_isTargetStillInRadius)
        {
            stateMachine.ChangeState(commonAnimal.idleState);
        }
    }
}