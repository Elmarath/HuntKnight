using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// This is an example of a base state. It is not used in the project.
public class DeathState : State
{
    // Condition variables set here

    public DeathState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        commonAnimal.stateLock = true;
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.DEATH);
        commonAnimal.agent.velocity *= 0f;
        commonAnimal.isDead = true;
        commonAnimal.gameObject.layer = LayerMask.NameToLayer("DeadAnimal");
        foreach (Transform child in commonAnimal.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer("DeadAnimal"); ;
        }
        commonAnimal.fieldOfView.isFieldOfViewVisible = false;
        commonAnimal.GetComponent<NavMeshAgent>().enabled = false;
        commonAnimal.GetComponentInChildren<FieldOfView>().enabled = false;
        //commonAnimal.GetComponentInChildren<AnimalCanvasController>().enabled = false;
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