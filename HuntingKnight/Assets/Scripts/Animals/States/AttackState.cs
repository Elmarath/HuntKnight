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
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.ATTACK);
        _timeWhenEnteredState = Time.time;
        Collider[] targetsInAttackRadius = Physics.OverlapSphere(commonAnimal.agent.transform.position, 2f);
        if (targetsInAttackRadius.Length > 0 && AnimalNavigationHelper.IsLayerInLayerMask(targetsInAttackRadius[0].gameObject.layer, commonAnimal.animalAttributes.attackThese))
        {
            _target = targetsInAttackRadius[0].transform;
            AnimalNavigationHelper.GoDestination(commonAnimal.agent, _target.position);
        }
        else
        {
            _target = null;
        }

        // When entered set conditions for exiting the state
    }

    public override void Exit()
    {
        base.Exit();
        commonAnimal.isAttacking = false;
        _isTargetStillInRadius = false;
        // When exiting set the animation variables
    }
    public override void HandleInput()
    {
        base.HandleInput();
        _isWaitTimeOver = (Time.time - _timeWhenEnteredState) >= commonAnimal.animalAttributes.attackSpeed;

        if (_isWaitTimeOver)
        {
            Collider[] targetsInAttackRadius = Physics.OverlapSphere(commonAnimal.agent.transform.position, commonAnimal.animalAttributes.sightRange, commonAnimal.animalAttributes.attackThese);
            if (targetsInAttackRadius.Length > 0)
            {
                _target = targetsInAttackRadius[0].transform;
                _isTargetStillInRadius = true;
            }
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (_isWaitTimeOver && _isTargetStillInRadius)
        {
            stateMachine.ChangeState(commonAnimal.idleState);
        }
        else if (_isWaitTimeOver)
        {
            stateMachine.ChangeState(commonAnimal.idleState);
        }
    }
}