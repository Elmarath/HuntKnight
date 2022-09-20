using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private float waitTime;
    private float timeWhenStateIsEntered = 0f;
    private bool isWaitTimeOver = false;

    private Rabbit rabbit; // state owner

    public Idle(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rabbit = animal.GetComponent<Rabbit>();
        isWaitTimeOver = false;
        waitTime = animal.idleTime; // normally you have to get this value by using GetComponent<AnimalKind>().idleTime
        timeWhenStateIsEntered = Time.time;

        rabbit.goIdle = true;
    }

    public override void Exit()
    {
        base.Exit();
        rabbit.goIdle = false;
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

        // has it seen nutrient -> go eat/drink
        if(rabbit.hasSeenNutrient && rabbit.isNeedNutrient)
        {   
            rabbit.walk.SetWalkDestination(rabbit.visibleNutrients[0].transform.position);
            stateMachine.ChangeState(rabbit.walk);
        }

        // is ready to poop
        else if(rabbit.isReadyToPoop)
        {
            // stateMachine.ChangeState(rabbit.poop);
            if(isWaitTimeOver)
            {
                stateMachine.ChangeState(rabbit.poop);
            }
        }

        // maybe mate condition goes here

        // if rabbit is not hungry and not ready to poop, then wait a while in idle
        else if(isWaitTimeOver)
        {
            stateMachine.ChangeState(rabbit.walk);

        }
    }

}