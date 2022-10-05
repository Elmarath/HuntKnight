using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is an example of a base state. It is not used in the project.
public class EatState : State
{
    // Condition variables set here
    private Transform _foodToEat;

    private float _timeWhenEnteredState;
    private bool _isWaitTimeOver;
    private bool _hasSuccessfullyEaten;

    public EatState(CommonAnimal commonAnimal, StateMachine stateMachine) : base(commonAnimal, stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        commonAnimal.animations.PlayAnimation(commonAnimal.animations.EAT);

        _timeWhenEnteredState = Time.time;
        _isWaitTimeOver = false;

        _hasSuccessfullyEaten = false;

        Collider[] targetsInEatRadius = Physics.OverlapSphere(commonAnimal.agent.transform.position, commonAnimal.animalAttributes.attackRange, commonAnimal.animalAttributes.eatThese); // generally attack range is the same as eat range
        Collider targetCollider = AnimalNavigationHelper.GetClosestCollider(commonAnimal.agent, targetsInEatRadius);
        if (targetCollider != null)
        {
            _foodToEat = targetCollider.transform;
            Debug.Log(_foodToEat.gameObject.name);
            // communication with target
            if (_foodToEat.GetComponent<EatableVegatation>() != null)
            {
                _foodToEat.GetComponent<EatableVegatation>().StartEating(commonAnimal);
                _hasSuccessfullyEaten = true;
            }
            else if (_foodToEat.GetComponentInParent<CommonAnimal>() != null)
            {
                _foodToEat.GetComponentInParent<CommonAnimal>().GetEaten();
                _hasSuccessfullyEaten = true;
            }
            commonAnimal.agent.isStopped = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        _foodToEat = null;
        commonAnimal.agent.isStopped = false;

        if (_hasSuccessfullyEaten)
        {
            commonAnimal.currentNutrientNeed = 0;
        }

        // When exiting set the animation variables
    }
    public override void HandleInput()
    {
        base.HandleInput();
        _isWaitTimeOver = (Time.time - _timeWhenEnteredState) >= commonAnimal.animalAttributes.consumeTime;
        // Handle the input and set conditions for exiting the state
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