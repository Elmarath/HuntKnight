using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class Poop : State
{
    // Condition variables set here
    private Rabbit rabbit; // state owner
    private float timeWhenStateIsEntered = 0f;
    private float poopTime;
    private bool isPoopTimeOver = false;

    public Poop(Animal animal, StateMachine stateMachine) : base(animal, stateMachine)
    {
    }
    //******* MUST BE FILLED*******//
    public override void Enter()
    {
        base.Enter();
        Debug.Log("Poop Enter");
        rabbit = animal.GetComponent<Rabbit>();
        poopTime = rabbit.poopTime;
        timeWhenStateIsEntered = Time.time;
        MakePoop(rabbit.animalExcrement, rabbit.transform.position);
        animal.currentState = "Poop";
        // Set Animation Variables
        rabbit.goPoop = true;

        // When entered set the animation variables (generally use GetComponent<AnimalKind>().variableName)
        // When entered set conditions for exiting the state
    }
    //******* MUST BE FILLED*******//
    public override void Exit()
    {
        base.Exit();
        rabbit.poopNeed = 0f;
        isPoopTimeOver = false;
        rabbit.goPoop = false;
        rabbit.goIdle = true;
        rabbit.nutrientNeed = 0f;
        // When exiting set the animation variables
    }
    //******* MUST BE FILLED*******//
    public override void HandleInput()
    {
        base.HandleInput();
        // Handle the input and set conditions for exiting the state
        if ((Time.time - timeWhenStateIsEntered) > poopTime)
        {
            isPoopTimeOver = true;
        }
    }
    //******* MUST BE FILLED*******//
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isPoopTimeOver)
        {
            stateMachine.ChangeState(rabbit.idle);
        }
    }

    // //******* State Specific Methods *******//
    public void MakePoop(GameObject excrement, Vector3 position)
    {
        GameObject _excrement = GameObject.Instantiate(excrement, position, Quaternion.identity);
        GameObject.Destroy(_excrement, 30f);
    }
}