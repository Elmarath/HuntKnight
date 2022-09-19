using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Animal : MonoBehaviour
{
    #region LogicVeriables
    [SerializeField]
    private float logicUpdateInterval = 0.25f;
    #endregion

    [Header("RabbitAttributes")]
    #region AnimalAttributes
    public float walkingSpeed;
    public float idleTime;
    [Range(0.31f, 1f)]
    public float closeEnoughTolerance;
    [Range(1, 20)]
    public float minSearchDistance; // min distance to search for anything
    #endregion
    
    [Header("FieldOfView")]
    #region FieldOfView
    [Range(1, 30)]
    public float viewRadius = 5f;
    [Range(1, 360)]
    public float viewAngle = 120f;
    LayerMask runAwayMask;
    public List<GameObject> visibleTargets = new List<GameObject>();
    #endregion

    [Header("Indicators")]
    #region
    public bool isIndicatorsWanted = false;
    public GameObject movementIndicator;
    #endregion

    [Header("TargetLayerMask")]
    #region LayerMasks
    public LayerMask targetMask;
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
        
        fieldOfView.viewRadius = viewRadius;
        fieldOfView.viewAngle = viewAngle;
        fieldOfView.targetMask = targetMask;

        SetAnimationNamesToIds();
        InitializeStates();
        InvokeRepeating("UpdateStateMachine", 0.1f, logicUpdateInterval);
        InvokeRepeating("UpdateFieldOfView", 0.1f, logicUpdateInterval);
    }

    public virtual void InitializeStates()
    {
        //stateMachine.Initialize(idle);
    }

    public virtual void UpdateStateMachine() 
    {
        UpdateState();
        stateMachine.CurrentState.HandleInput();
        stateMachine.CurrentState.LogicUpdate();
        UpdateAnimationVariables();
    }

    // Updates field of view so it can see targets
    public virtual void UpdateFieldOfView()
    {
        visibleTargets = fieldOfView.FindVisibleTargets();
    }

    // Updates the animation variables
    public virtual void UpdateAnimationVariables()
    {
    }

    // Update state checks for switching to another state
    // must be overriden in child class if instant state checks are needed
    public virtual void UpdateState()
    {
    }

    // Must be initialized to set animation string to ids for better performance
    public virtual void SetAnimationNamesToIds()
    {
    }

    // Creates a random Destination which in the animals searchable area
   public Vector3 CreateRandomDestination(float viewRadius, float viewAngle, float minSearchDistance)
    {
        Vector3 randomDirection;
        Vector2 randomDirectionV2;
        Vector3 finalPosition;
        NavMeshHit hit;

        for(int i = 0; i < 10; i++)
        {
            randomDirectionV2 = Random.insideUnitCircle * viewRadius;
            randomDirection = new Vector3(randomDirectionV2.x, 0, randomDirectionV2.y);
            randomDirection += transform.position;

            float distance = Vector3.Distance(randomDirection, transform.position);

            // Distance Check
            if(distance < minSearchDistance)
            {
                continue;
            }
            // Angle Check
            if(Vector3.Angle(transform.forward, randomDirection - transform.position) > viewAngle / 2)
            {
                continue;
            }
            // Navigation Validation Check
            bool isWalkable = NavMesh.SamplePosition(randomDirection, out hit, 0.3f, NavMesh.AllAreas);
            if(!isWalkable)
            {
                continue;
            }

            finalPosition = randomDirection;
            
            if(isIndicatorsWanted)
            {
                SpawnIndicator(movementIndicator, finalPosition);
            }

            return finalPosition;
        }

        finalPosition = transform.position - (transform.forward * minSearchDistance);
        return finalPosition;
    }

    // sets given destination to the agent's destination and draws ray to that position
    public void GotoDestination(Vector3 destination)
    {
        Vector3 dir = destination - transform.position;
        Debug.DrawRay(transform.position, dir, Color.red, 2f, false);
        agent.SetDestination(destination);
    }

    // Check if self is close enough to destination
    public bool IsCloseEnough(Vector3 destination, float tolerance)
    {
        Vector3 _position = new Vector3(transform.position.x, 0, transform.position.z);
        return  ((destination - _position).sqrMagnitude) < (tolerance * tolerance);
    }

    // Spawns the given indicator in position
    public void SpawnIndicator(GameObject indicator, Vector3 position)
    {
        GameObject _indicator = Instantiate(indicator, position, Quaternion.identity);
        Destroy(_indicator, 5f);
    }
}
