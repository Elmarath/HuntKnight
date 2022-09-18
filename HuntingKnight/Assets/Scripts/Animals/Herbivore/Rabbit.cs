using UnityEngine;

public class Rabbit : Herbivore
{
    #region AnimalAttributes
    #endregion

    #region FieldOfView
    LayerMask targetMask;
    #endregion

    #region StateVeriables
    [HideInInspector]
    public Idle idle;
    [HideInInspector]
    public Walk walk;
    #endregion

    #region animationStringIds
    private int goIdle_id;
    private int goWalk_id;
    #endregion

    // Control Variables for state machine
    #region stateChecks
    [HideInInspector]
    public bool goIdle = false;
    [HideInInspector]
    public bool goWalk = false;
    [HideInInspector]
    public bool hasSeenFood = false;
    #endregion

    void Awake()
    {
        Initialize();
    }

    //******* MUST BE FILLED*******//
    // Updates instant state checks (like seen predetor, food, or got damaged)
    public override void UpdateState()
    {
        base.UpdateState();
        // is food visible

        // is predator visible

        // is walking

        // is idle

        // is eating

        // is drinking
    }

    //******* MUST BE FILLED*******//
    // This method is used to update the animation variables of the animal
    // Base updates goIdle, goWalk
    public override void UpdateAnimationVariables(){
        base.UpdateAnimationVariables();
        animator.SetBool(goIdle_id, goIdle);
        animator.SetBool(goWalk_id, goWalk);
    }

    //******* MUST BE FILLED*******//
    // Must be initialized to set animation string to ids for better performance
    // set your animation id's using Animator.StringToHash("AnimationName") and use the id
    public override void SetAnimationNamesToIds()
    {
        base.SetAnimationNamesToIds();
        goIdle_id = Animator.StringToHash("goIdle");
        goWalk_id = Animator.StringToHash("goWalk");
    }


    //******* MUST BE FILLED*******//
    // This method takes the states of the animal and initalizes them
    // Initialize method calls this method
    public override void InitializeStates()
    {
        base.InitializeStates();
        idle = new Idle(this, stateMachine);
        walk = new Walk(this, stateMachine);
        stateMachine.Initialize(idle);
    }


    private bool isFoodVisible()
    {
        if(visibleTargets.Count > 0)
        {
            for( int i = 0; i < visibleTargets.Count; i++)
            {
                if(isLayerInLayerMask(visibleTargets[i].gameObject.layer, targetMask))
                {
                    return true;
                }
            }
        }
        return false;
    }


    private bool isLayerInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

}