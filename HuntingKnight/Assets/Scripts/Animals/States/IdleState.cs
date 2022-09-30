using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class IdleState : State
{
    // Condition variables set here
    private float _idleTime;
    private float _timeWhenEnteredState;
    private bool _isWaitTimeOver;
    private bool _isForwardWalkable;

    public IdleState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered Idle State");
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.IDLE);
        _idleTime = commonAnimal.animalAttributes.idleTime;
        _timeWhenEnteredState = Time.time;
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void HandleInput()
    {
        base.HandleInput();
        _isWaitTimeOver = (Time.time - _timeWhenEnteredState) >= _idleTime;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (_isWaitTimeOver)
        {
            stateMachine.ChangeState(commonAnimal.walkState);
        }
    }

}