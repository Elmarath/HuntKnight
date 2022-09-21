using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class Consume : State
{
    // Condition variables set here
    private Rabbit rabbit; // state owner
    private float consumeTime;
    private float timeWhenStateIsEntered = 0f;
    private bool isConsumeTimeOver = false;

    public Consume(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
    {
    }
    //******* MUST BE FILLED*******//
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Consume Enter");
        rabbit = animal.GetComponent<Rabbit>();
        consumeTime = rabbit.consumeTime;
        timeWhenStateIsEntered = Time.time;

        animal.currentState = "Consume";
        // Set Animation Variables
        rabbit.goConsume = true;

        // When entered set the animation variables (generally use GetComponent<AnimalKind>().variableName)
        // When entered set conditions for exiting the state
    }
    //******* MUST BE FILLED*******//
    public override void Exit()
    {
        base.Exit();
        isConsumeTimeOver = false;
        rabbit.goConsume = false;
        rabbit.goIdle = true;
        rabbit.nutrientNeed = 0f;
        // When exiting set the animation variables
    }
    //******* MUST BE FILLED*******//
    public override void HandleInput()
    {
        base.HandleInput();
        // Handle the input and set conditions for exiting the state
        if ((Time.time - timeWhenStateIsEntered) > consumeTime)
        {
            isConsumeTimeOver = true;
        }
    }
    //******* MUST BE FILLED*******//
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isConsumeTimeOver)
        {
            stateMachine.ChangeState(rabbit.idle);
        }

        // // check if consumable still exists
        // if(!(animal.visibleNutrients.Count > 0)) // if not
        // {
        //     Debug.Log("No more consumable");
        //     stateMachine.ChangeState(rabbit.idle);
        // }
        // else
        // {
        //     Debug.Log("Consumable still exists");
        //     stateMachine.ChangeState(rabbit.consume);
        // }
    }
}