using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private float waitTime;
    private float timeWhenStateIsEntered = 0f;
    private bool isWaitTimeOver = false;

    private State rabbitWalkState;

    public Idle(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rabbitWalkState = animal.GetComponent<Rabbit>().walk;
        isWaitTimeOver = false;
        waitTime = animal.idleTime; // normally you have to get this value by using GetComponent<AnimalKind>().idleTime
        timeWhenStateIsEntered = Time.time;

        animal.GetComponent<Rabbit>().goIdle = true;
    }

    public override void Exit()
    {
        base.Exit();
        animal.GetComponent<Rabbit>().goIdle = false;
    }
    public override void HandleInput()
    {
        base.HandleInput();
        if((Time.time - timeWhenStateIsEntered) > waitTime)
        {
            isWaitTimeOver = true;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if(isWaitTimeOver)
        {
            stateMachine.ChangeState(rabbitWalkState);
        }
    }

}