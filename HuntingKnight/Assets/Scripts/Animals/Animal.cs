using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animal : MonoBehaviour
{
    #region LogicVeriables
    [Range(0.1f, 1f)]
    public float logicUpdateInterval = 0.25f; // how often the animal will update its logic
    public string currentState; // current state of the animal
    #endregion

    [Header("Animal Needs")]
    #region AnimalNeeds
    [Range(1, 100)]
    public float nutrientNeed = 0f;
    [Range(1, 100)]
    public float poopNeed = 0f;
    public float nutrientNeedCapacity = 100f;
    public float poopNeedCapacity = 100f;
    public float nutrientNeedIncreaseRate = 1f; // per sec
    public float poopNeedIncreaseRate = 1f; // per sec
    public float needNutrientThreshold = 30f;
    public float needToPoopThreshold = 90f;
    [Space(20)]
    #endregion

    [Header("RabbitMovementAttributes")]
    #region AnimalAttributes
    public float walkingSpeed;
    public float runningSpeed;
    public float idleTime;
    public float consumeTime;
    public float poopTime;
    [Range(0.1f, 1f)]
    public float closeEnoughTolerance;
    [Range(1, 20)]
    public float minSearchDistance; // min distance to search for anything
    #endregion

    [Header("FieldOfView")]
    #region FieldOfView
    [HideInInspector] private List<GameObject> visibleTargets = new List<GameObject>();
    public List<GameObject> visibleNutrients = new List<GameObject>();
    public List<GameObject> visibleRunFroms = new List<GameObject>();
    [Range(1, 30)]
    public float viewRadius = 5f;
    [Range(1, 360)]
    public float viewAngle = 120f;
    [HideInInspector] public LayerMask targetMask;
    [Space(20)]
    #endregion

    [Header("Indicators")]
    #region
    public bool isIndicatorsWanted = false;
    public GameObject movementIndicator;
    public GameObject animalExcrement;
    #endregion

    [Header("TargetLayerMask")]
    #region LayerMasks
    public LayerMask nutrientMask; // layer mask for food and drinks
    public LayerMask runFromMask; // Animal is wary of this layer gameobjects and will run away from them
    #endregion

    #region Attachments
    [HideInInspector]
    public StateMachine stateMachine;
    [HideInInspector]
    public NavMeshAgent agent;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public FieldOfView fieldOfView;
    #endregion

    // Control Variables for state machine
    #region stateChecks    
    #endregion

    #region animationStringIds
    #endregion

    public virtual void Initialize()
    {
        stateMachine = new StateMachine();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        fieldOfView = GetComponent<FieldOfView>();

        targetMask = AnimalHelper.AddTwoMask(runFromMask, nutrientMask);
        fieldOfView.viewRadius = viewRadius;
        fieldOfView.viewAngle = viewAngle;
        fieldOfView.targetMask = targetMask;

        // set navmesh values
        agent.speed = walkingSpeed;

        SetAnimationNamesToIds();
        InitializeStates();
        InvokeRepeating("UpdateAnimalNeeds", 0f, logicUpdateInterval); // 1st
        InvokeRepeating("UpdateStateChecks", 0f, logicUpdateInterval); // 2nd
        InvokeRepeating("UpdateStateMachine", 0f, logicUpdateInterval); // 3rd
        InvokeRepeating("UpdateFieldOfView", 0f, logicUpdateInterval); // 4th
    }

    public virtual void InitializeStates()
    {
        //stateMachine.Initialize(idle);
    }

    public virtual void UpdateStateMachine()
    {
        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.LogicUpdate();
        UpdateAnimationVariables();
    }

    // Updates field of view so it can see targets
    // Called every logicUpdateInterval
    public virtual void UpdateFieldOfView()
    {
        visibleTargets = fieldOfView.FindVisibleTargets();
    }

    // Updates the animation variables
    // Called every logicUpdateInterval
    public virtual void UpdateAnimationVariables()
    {
    }

    // Must be initialized to set animation string to ids for better performance
    // only called once
    public virtual void SetAnimationNamesToIds()
    {
    }

    // Called every logicUpdateInterval
    public virtual void UpdateAnimalNeeds() // updates the animals needs
    {
        // Update needs
        nutrientNeed += nutrientNeedIncreaseRate * logicUpdateInterval;
        poopNeed += poopNeedIncreaseRate * logicUpdateInterval;

        // Clamp needs
        nutrientNeed = Mathf.Clamp(nutrientNeed, 0f, nutrientNeedCapacity);
        poopNeed = Mathf.Clamp(poopNeed, 0f, poopNeedCapacity);
    }

    // In this method the animal will check if it needs to do something (like eat, drink, poop, run etc)
    // Called every logicUpdateInterval
    public virtual void UpdateStateChecks()
    {
    }

    public virtual void HandleInterrupts()
    {
    }
}
