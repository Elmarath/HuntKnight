using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CommonAnimal : MonoBehaviour
{

    #region LogicVeriables
    [SerializeField, Tooltip("How often logic will be updated. High value for performance"), Range(0.1f, 1f)]
    private float logicUpdateInterval = 0.25f; // how often the animal will update its logic
    #endregion

    [Header("Animal Attributes")]
    public CommonAnimalAttributes animalAttributes;

    [Header("FieldOfView")]
    #region FieldOfView
    [HideInInspector] private List<GameObject> visibleRunFromThese = new List<GameObject>();
    [HideInInspector] private List<GameObject> visibleAttackThese = new List<GameObject>();
    [HideInInspector] private List<GameObject> visibleEatThese = new List<GameObject>();
    [HideInInspector] private LayerMask targetMask;
    #endregion

    #region Attachments
    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    [HideInInspector] public FieldOfView fieldOfView;
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
    [HideInInspector] public bool isIdling;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isWalking;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool isEating;
    [HideInInspector] public bool isExcreting;
    [HideInInspector] public bool isMating;
    [HideInInspector] public bool isMakingBirth;
    [HideInInspector] public bool isTakingDamage;
    [HideInInspector] public bool isTakingCover;
    [HideInInspector] public bool isCustom;
    [HideInInspector] public bool isStateFinished;
    #endregion

    [HideInInspector] public Vector3 walkToPosition = Vector3.zero;

    private void Awake()
    {
        Initialize();
        InvokeRepeating("UpdateStateMachine", 0f, logicUpdateInterval);
        InvokeRepeating("UpdateAnimations", 0f, logicUpdateInterval);
    }

    public void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fieldOfView = GetComponentInChildren<FieldOfView>();
        stateMachine = new StateMachine();
        targetMask = animalAttributes.runFromThese | animalAttributes.attackThese | animalAttributes.eatThese;
        fieldOfView.viewRadius = animalAttributes.sightRange;
        fieldOfView.viewAngle = animalAttributes.sightAngle;
        agent.speed = animalAttributes.walkSpeed;
        agent.angularSpeed = 240f;

        InitializeStates();
    }

    public void UpdateVisibleTargets()
    {
        visibleRunFromThese.Clear();
        visibleAttackThese.Clear();
        visibleEatThese.Clear();
        foreach (GameObject target in fieldOfView.visibleTargets)
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

    public void InitializeStates()
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

    public void UpdateStateMachine()
    {
        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.LogicUpdate();
        stateMachine.CurrentState.HandleInterrupt();
    }

    public int[] GetAnimationParameters()
    {
        int[] animatorParameters = new int[animator.parameterCount];
        for (int i = 0; i < animator.parameterCount; i++)
        {
            animatorParameters[i] = animator.GetParameter(i).nameHash;
        }
        return animatorParameters;
    }

    public void UpdateAnimations()
    {
        int[] animatorParameters = GetAnimationParameters();
        animator.SetBool(animatorParameters[0], stateMachine.CurrentState == idleState);
        animator.SetBool(animatorParameters[1], stateMachine.CurrentState == deathState);
        animator.SetBool(animatorParameters[2], stateMachine.CurrentState == attackState);
        animator.SetBool(animatorParameters[3], stateMachine.CurrentState == walkState);
        animator.SetBool(animatorParameters[4], stateMachine.CurrentState == runState);
        animator.SetBool(animatorParameters[5], stateMachine.CurrentState == eatState);
        animator.SetBool(animatorParameters[6], stateMachine.CurrentState == excreteState);
        animator.SetBool(animatorParameters[7], stateMachine.CurrentState == mateState);
        animator.SetBool(animatorParameters[8], stateMachine.CurrentState == makeBirthState);
        animator.SetBool(animatorParameters[9], stateMachine.CurrentState == takeDamageState);
        animator.SetBool(animatorParameters[10], stateMachine.CurrentState == customState);
        animator.SetBool(animatorParameters[11], isStateFinished);
    }

    public void HandleInterrupt()
    {

    }
}
