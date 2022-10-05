using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class ExcreteState : State
{
    // Condition variables set here
    private float _excreteTime;
    private float _timeWhenEnteredState;
    private bool _isWaitTimeOver;

    public ExcreteState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.EXCRETE);
        commonAnimal.agent.velocity = Vector3.zero;
        _excreteTime = commonAnimal.animalAttributes.poopTime;
    }

    public override void Exit()
    {
        base.Exit();
        GameObject.Instantiate(commonAnimal.animalAttributes.animalExcrete, commonAnimal.agent.transform.position - commonAnimal.agent.transform.forward / 2, Quaternion.identity);
        commonAnimal.currentPoopNeed = 0f;
    }
    public override void HandleInput()
    {
        base.HandleInput();

        _isWaitTimeOver = (Time.time - _timeWhenEnteredState) >= _excreteTime;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (_isWaitTimeOver)
        {
            stateMachine.ChangeState(commonAnimal.idleState);
        }

        // Check the conditions for exiting the state if true change the state
    }

}