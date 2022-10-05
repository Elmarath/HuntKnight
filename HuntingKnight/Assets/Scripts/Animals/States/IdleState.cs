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
    private bool _isGoingForNutrient;

    private bool _isPoopTime;

    public IdleState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        commonAnimal.agent.velocity = Vector3.zero;
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.IDLE);
        _idleTime = commonAnimal.animalAttributes.idleTime;
        _timeWhenEnteredState = Time.time;
        commonAnimal.agent.isStopped = true;

        Debug.Log("IdleState");

        _isGoingForNutrient = commonAnimal.isGoingForNutrient;
        if (_isGoingForNutrient)
        {
            if (commonAnimal.visibleEatThese.Count > 0)
            {
                Vector3 walkToEatPosition = commonAnimal.visibleEatThese[0].transform.position - (commonAnimal.visibleEatThese[0].transform.position - commonAnimal.transform.position).normalized;
                commonAnimal.walkToPosition = walkToEatPosition;
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
        commonAnimal.agent.isStopped = false;
    }
    public override void HandleInput()
    {
        base.HandleInput();
        _isWaitTimeOver = (Time.time - _timeWhenEnteredState) >= _idleTime;

        if (_isWaitTimeOver)
        {
            _isPoopTime = commonAnimal.isPoopNeedCritical;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (_isWaitTimeOver)
        {
            stateMachine.ChangeState(commonAnimal.walkState);
        }

        if (_isWaitTimeOver && _isPoopTime)
        {
            stateMachine.ChangeState(commonAnimal.excreteState);
        }
    }

}