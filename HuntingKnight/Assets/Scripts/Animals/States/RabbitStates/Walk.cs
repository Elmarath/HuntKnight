using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : State
{
    private bool isArrived = false;
    private Vector3 walkDestination;
    private State rabbitIdleState;

    public Walk(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rabbitIdleState = animal.GetComponent<Rabbit>().idle;
        // Set Animation Variables
        animal.GetComponent<Rabbit>().goWalk = true;
        walkDestination = animal.CreateRandomDestination(animal.viewRadius, animal.viewAngle, animal.minSearchDistance);
        walkDestination.y = 0f;
        animal.GotoDestination(walkDestination);
    }

    public override void Exit()
    {
        base.Exit();
        // Set Animation Variables
        animal.GetComponent<Rabbit>().goWalk = false;
    }
    public override void HandleInput()
    {
        base.HandleInput();
        isArrived = animal.IsCloseEnough(walkDestination, animal.closeEnoughTolerance);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (isArrived)
        {
            stateMachine.ChangeState(rabbitIdleState);
        }
    }

}