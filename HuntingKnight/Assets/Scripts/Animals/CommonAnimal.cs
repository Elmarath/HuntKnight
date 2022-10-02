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
    [HideInInspector] public State fleeState;
    [HideInInspector] public State chaseState;
    [HideInInspector] public State takeDamageState;
    [HideInInspector] public State walkState;
    [HideInInspector] public State takeCoverState;
    [HideInInspector] public bool stateLock;
    #endregion

    // for changing states and their animations 
    // (not the state we are in but the state we are going to be in)
    #region StateCondisitons
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool isAttacking;
    [HideInInspector] public bool isChasing;
    [HideInInspector] public bool isFleeing;
    [HideInInspector] public bool isTakingDamage;
    [HideInInspector] public bool isTakingCover;
    [HideInInspector] public bool isCustom;
    #endregion



    #region animalStats
    [HideInInspector] public float currentHealth;
    [HideInInspector] public float currentStamina;
    [HideInInspector] public float currentNutrientNeed;
    [HideInInspector] public float currentPoopNeed;
    [HideInInspector] public bool isHealthDepleted;
    [HideInInspector] public bool isStaminaDepleted;
    [HideInInspector] public bool isNutrientNeedCritical;
    [HideInInspector] public bool isPoopNeedCritical;

    [HideInInspector] public bool isStaminaBeeingUsed;
    [HideInInspector] public Vector3 walkToPosition = Vector3.zero;


    private GameObject _canvas;

    #endregion

    private void Awake()
    {
        Initialize();
        _canvas = transform.Find("Canvas").gameObject;
        _canvas.SetActive(false);
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
        agent.angularSpeed = animalAttributes.angularSpeed;

        currentHealth = animalAttributes.health;
        currentStamina = animalAttributes.stamina;
        currentNutrientNeed = 0f;
        currentPoopNeed = 0f;

        InitializeStates();
    }

    public virtual void UpdateInterruptStates()
    {
        UpdateVisibleTargets();
        UpdateAnimalStats();

        if (visibleRunFromThese.Count > 0 && !isStaminaDepleted)
        {
            isFleeing = true;
        }
        else if (visibleAttackThese.Count > 0 && !isStaminaDepleted)
        {
            isChasing = true;
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

    public virtual void UpdateAnimalStats()
    {
        currentNutrientNeed += animalAttributes.nutrientIncreaseRate * logicUpdateInterval;
        currentPoopNeed += animalAttributes.poopIncreaseRate * logicUpdateInterval;
        if (!isStaminaBeeingUsed)
        {
            currentStamina += animalAttributes.staminaIncreaseRate * logicUpdateInterval;
        }
        else
        {
            currentStamina -= animalAttributes.staminaDecreaseRate * logicUpdateInterval;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0f, animalAttributes.health);
        currentStamina = Mathf.Clamp(currentStamina, 0f, animalAttributes.stamina);
        currentNutrientNeed = Mathf.Clamp(currentNutrientNeed, 0f, animalAttributes.nutrientCapacity);
        currentPoopNeed = Mathf.Clamp(currentPoopNeed, 0f, animalAttributes.holdingPoopCapacity);

        isHealthDepleted = currentHealth <= 0f;
        isStaminaDepleted = currentStamina <= (animalAttributes.stamina / 5f);
        isNutrientNeedCritical = currentNutrientNeed >= animalAttributes.nutientThreshold;
        isPoopNeedCritical = currentPoopNeed >= animalAttributes.poopThreshold;
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
        chaseState = new ChaseState(this, stateMachine);
        fleeState = new FleeState(this, stateMachine);
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
        if (!stateLock)
        {
            if (isDead && stateMachine.CurrentState != deathState)
            {
                stateMachine.ChangeState(deathState);
            }
            else if (isTakingDamage && stateMachine.CurrentState != takeDamageState)
            {
                stateMachine.ChangeState(takeDamageState);
            }
            else if (isFleeing && stateMachine.CurrentState != fleeState)
            {
                stateMachine.ChangeState(fleeState);
            }
            else if (isChasing && stateMachine.CurrentState != chaseState)
            {
                stateMachine.ChangeState(chaseState);
            }
            else if (isTakingCover && stateMachine.CurrentState != takeCoverState)
            {
                stateMachine.ChangeState(takeCoverState);
            }
            else if (isCustom && stateMachine.CurrentState != customState)
            {
                stateMachine.ChangeState(customState);
            }
        }
    }

    public virtual void ToggleCanvas()
    {
        if (_canvas != null)
        {
            _canvas.SetActive(!_canvas.activeSelf);
        }
    }
}
