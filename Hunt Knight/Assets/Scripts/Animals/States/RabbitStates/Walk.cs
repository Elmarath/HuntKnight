using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : State // walks to a random destination if no destination given from animal -> walkDestination
{
    private bool isArrived = false;
    private Vector3 walkDestination;
    private State rabbitIdleState;
    private bool isDestinationSet = false;

    private Rabbit rabbit; // state owner

    // if walkDestination is zero, then walk to a random destination
    public Walk(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rabbit = animal.GetComponent<Rabbit>();
        // Set Animation Variables
        rabbit.goWalk = true;

        // if Destination not set, then set a random destination
        if(!isDestinationSet)
        {
            walkDestination = animal.CreateRandomDestination(animal.viewRadius, animal.viewAngle, animal.minSearchDistance);
            walkDestination.y = 0f;
        }
        
        walkDestination.y = 0f;
        animal.GotoDestination(walkDestination);
    }
    public override void HandleInput()
    {
        base.HandleInput();
        // check while walking seen a nutrient and is it needed        
        if(rabbit.hasSeenNutrient && rabbit.isNeedNutrient)
        {
            SetWalkDestination(rabbit.visibleNutrients[0].transform.position);
        }
        
        Vector3 _position = new Vector3(rabbit.transform.position.x, 0, rabbit.transform.position.z);

        isArrived = animal.IsCloseEnough(walkDestination, animal.closeEnoughTolerance);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        // if is hungry and has seen nutrient and close enough -> go eat/drink
        // see if food is still avalible
        if(rabbit.isNeedNutrient){
            if(rabbit.visibleNutrients.Count > 0){
                if(isArrived)
                {
                    stateMachine.ChangeState(rabbit.consume);
                    return;
                }
            }
        }

        if (isArrived)
        {
            stateMachine.ChangeState(rabbit.idle);
        }
    }

    public override void Exit()
    {
        base.Exit();
        // Set Animation Variables
        rabbit.goWalk = false;
        isDestinationSet = false;
    }

    // Call This method if you want to walk to a specific destination if you dont want to walk to a random destination
    public void SetWalkDestination(Vector3 destination)
    {
        isDestinationSet = true;
        walkDestination = destination;
        walkDestination.y = 0f;
    }

}