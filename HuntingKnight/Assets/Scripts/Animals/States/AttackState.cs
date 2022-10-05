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
        /////
        commonAnimal.stateLock = true;
        /////
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.ATTACK);
        _timeWhenEnteredState = Time.time;
        _isWaitTimeOver = false;

        commonAnimal.agent.velocity *= 0.01f;

        Collider[] targetsInAttackRadius = Physics.OverlapSphere(commonAnimal.agent.transform.position, commonAnimal.animalAttributes.attackRange, commonAnimal.animalAttributes.attackThese);
        Collider targetCollider = AnimalNavigationHelper.GetClosestCollider(commonAnimal.agent, targetsInAttackRadius);
        if (targetCollider != null)
        {
            _target = targetCollider.transform.parent;
            // communication with target
            if (_target.GetComponent<CommonAnimal>() != null)
            {
                _target.GetComponent<CommonAnimal>().TakeDamage(commonAnimal.animalAttributes.strength);
                commonAnimal.agent.destination = _target.position;
            }

        }
    }

    public override void Exit()
    {
        base.Exit();
        /////
        commonAnimal.stateLock = false;
        /////
        commonAnimal.agent.isStopped = false;
        commonAnimal.isAttacking = false;
        _isTargetStillInRadius = false;
        // When exiting set the animation variables
    }
    public override void HandleInput()
    {
        base.HandleInput();

        _isWaitTimeOver = (Time.time - _timeWhenEnteredState) >= commonAnimal.animalAttributes.attackSpeed;
        if (_target != null)
        {
            _isTargetStillInRadius = AnimalNavigationHelper.IsCloseEnough(commonAnimal.agent.transform.position, _target.position, commonAnimal.animalAttributes.sightRange);
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isWaitTimeOver && _isTargetStillInRadius && !_target.GetComponent<CommonAnimal>().isDead)
        {
            stateMachine.ChangeState(commonAnimal.chaseState);
        }
        if (_isWaitTimeOver && _isTargetStillInRadius && _target.GetComponent<CommonAnimal>().isDead)
        {
            stateMachine.ChangeState(commonAnimal.idleState);
        }
        else if (_isWaitTimeOver && !_isTargetStillInRadius)
        {
            stateMachine.ChangeState(commonAnimal.idleState);
        }
    }
}