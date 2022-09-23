using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class ExampleBaseState : State
{
    // Condition variables set here

    public ExampleBaseState(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // When entered set the animation variables (generally use GetComponent<AnimalKind>().variableName)
        // When entered set conditions for exiting the state
    }

    public override void Exit()
    {
        base.Exit();
        // When exiting set the animation variables
    }
    public override void HandleInput()
    {
        base.HandleInput();
        // Handle the input and set conditions for exiting the state
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        // Check the conditions for exiting the state if true change the state
    }

}