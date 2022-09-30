using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CommonAnimal : MonoBehaviour
{

    #region LogicVeriables
    [SerializeField, Tooltip("How often logic will be updated. High value for performance"), Range(0.1f, 1f)]
    private float logicUpdateInterval = 0.25f; // how often the animal will update its logic
    #endregion

    [Header("Animal Attributes")]
    public CommonAnimalAttributes animalAttributes;

    [Header("FieldOfView")]
    #region FieldOfView
    [HideInInspector] public List<GameObject> visibleRunFromThese = new List<GameObject>();
    [HideInInspector] public List<GameObject> visibleAttackThese = new List<GameObject>();
    [HideInInspector] public List<GameObject> visibleEatThese = new List<GameObject>();
    #endregion

    #region Attachments
    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public FieldOfView fieldOfView;
    [HideInInspector] public CommonAnimalAnimations animations;
    #endregion

    #region States
    [HideInInspector] public State attackState;
    [HideInInspector] public State customState;
    [HideInInspector] public State deathState;
    [HideInInspector] public State eatState;
    [HideInInspector] public State excreteState;
    [HideInInspector] public State idleState;
    [HideInInspector] public State makeBirthState;
    [HideInInspector] public State mateState;
    [HideInInspector] public State runState;
    [HideInInspector] public State takeDamageState;
    [HideInInspector] public State walkState;
    [HideInInspector] public State takeCoverState;
    #endregion

    // for changing states and their animations 
    // (not the state we are in but the state we are going to be in)
    #region StateCondisitons
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isTakingDamage;
    [HideInInspector] public bool isTakingCover;
    [HideInInspector] public bool isCustom;
    #endregion

    [HideInInspector] public Vector3 walkToPosition = Vector3.zero;

    private void Awake()
    {
        Initialize();
        InvokeRepeating("UpdateStateMachine", 0f, logicUpdateInterval);
        InvokeRepeating("UpdateInterruptStates", 0f, logicUpdateInterval);
    }

    public virtual void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fieldOfView = GetComponentInChildren<FieldOfView>();
        stateMachine = new StateMachine();
        animations = new CommonAnimalAnimations(animator);
        fieldOfView.targetMask = animalAttributes.runFromThese | animalAttributes.attackThese | animalAttributes.eatThese;
        fieldOfView.viewRadius = animalAttributes.sightRange;
        fieldOfView.viewAngle = animalAttributes.sightAngle;
        agent.speed = animalAttributes.walkSpeed;
        agent.angularSpeed = 240f;

        InitializeStates();
    }

    public virtual void UpdateInterruptStates()
    {
        UpdateVisibleTargets();
        if (visibleRunFromThese.Count > 0 || visibleAttackThese.Count > 0)
        {
            isRunning = true;
        }
    }

    public virtual void UpdateVisibleTargets()
    {
        visibleRunFromThese.Clear();
        visibleAttackThese.Clear();
        visibleEatThese.Clear();
        foreach (GameObject target in fieldOfView.FindVisibleTargets())
        {
            if (animalAttributes.runFromThese == (animalAttributes.runFromThese | (1 << target.layer)))
            {
                visibleRunFromThese.Add(target);
            }
            if (animalAttributes.attackThese == (animalAttributes.attackThese | (1 << target.layer)))
            {
                visibleAttackThese.Add(target);

            }
            if (animalAttributes.eatThese == (animalAttributes.eatThese | (1 << target.layer)))
            {
                visibleEatThese.Add(target);
            }
        }
    }

    public virtual void InitializeStates()
    {
        attackState = new AttackState(this, stateMachine);
        customState = new CustomState(this, stateMachine);
        deathState = new DeathState(this, stateMachine);
        eatState = new EatState(this, stateMachine);
        excreteState = new ExcreteState(this, stateMachine);
        idleState = new IdleState(this, stateMachine);
        makeBirthState = new MakeBirthState(this, stateMachine);
        mateState = new MateState(this, stateMachine);
        runState = new RunState(this, stateMachine);
        takeDamageState = new TakeDamageState(this, stateMachine);
        walkState = new WalkState(this, stateMachine);
        takeCoverState = new TakeCoverState(this, stateMachine);

        stateMachine.Initialize(idleState);
    }

    public virtual void UpdateStateMachine()
    {
        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.LogicUpdate();
        stateMachine.CurrentState.HandleInterrupt();
    }

    public virtual void HandleInterrupt()
    {
        if (isDead)
        {
            stateMachine.ChangeState(deathState);
        }
        else if (isTakingDamage)
        {
            stateMachine.ChangeState(takeDamageState);
        }
        else if (isRunning)
        {
            stateMachine.ChangeState(runState);
        }
        else if (isAttacking)
        {
            stateMachine.ChangeState(attackState);
        }
        else if (isCustom)
        {
            stateMachine.ChangeState(customState);
        }
    }

}
