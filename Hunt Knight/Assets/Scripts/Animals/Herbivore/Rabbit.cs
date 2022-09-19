using UnityEngine;

public class Rabbit : Herbivore
{
    #region AnimalAttributes
    #endregion

    #region FieldOfView
    #endregion

    #region StateVeriables
        [HideInInspector]
        public Idle idle;
        [HideInInspector]
        public Walk walk;
        [HideInInspector]
        public Consume consume;
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
        public bool hasSeenNutrient = false;
        [HideInInspector]
        public bool hasSeenPredator = false;
        [HideInInspector]
        public bool isNeedNutrient = false;
        [HideInInspector]
        public bool isReadyToPoop = false;
    #endregion

    void Awake()
    {
        Initialize();
    }

    //******* MUST BE FILLED*******//
    // Update state checks for switching to another state in instant
    // This is for switching state check that are instant (From any state)
    // Called every logicUpdateInterval
    public override void UpdateState()
    {
        base.UpdateState();
        
        if(hasSeenPredator)
        {
            Debug.Log("Rabbit has seen predator");
        }
    }

    //******* MUST BE FILLED*******//
    // Called every logicUpdateInterval
    // This method is used to update the animation variables of the animal
    // Base updates goIdle, goWalk
    public override void UpdateAnimationVariables(){
        base.UpdateAnimationVariables();
        animator.SetBool(goIdle_id, goIdle);
        animator.SetBool(goWalk_id, goWalk);
    }

    //******* MUST BE FILLED*******//
    // Called every logicUpdateInterval
    // This method is for updating the stateChecks
    public override void UpdateStateChecks(){
        // is there anything visible in field of view
        hasSeenNutrient = false;
        hasSeenPredator = false;
        visibleNutrients.Clear();
        visibleRunFroms.Clear();
        if (fieldOfView.GetVisibleTargets().Count > 0)
        {
            for(int i = 0; i < fieldOfView.GetVisibleTargets().Count; i++)
            {
                // is a nutriment visible
                if (isLayerInLayerMask(fieldOfView.GetVisibleTargets()[i].gameObject.layer, nutrientMask))
                {
                    visibleNutrients.Add(fieldOfView.GetVisibleTargets()[i].gameObject);
                    hasSeenNutrient = true;
                }
                // is animal to run from visible 
                if(isLayerInLayerMask(fieldOfView.GetVisibleTargets()[i].gameObject.layer, runFromMask))
                {
                    visibleRunFroms.Add(fieldOfView.GetVisibleTargets()[i].gameObject);
                    hasSeenPredator = true;
                }
            }
        }

        isNeedNutrient = nutrientNeed > needNutrientThreshold;
        isReadyToPoop = poopNeed > needToPoopThreshold;
    }

    //******* MUST BE FILLED*******// 
    // only called once when the animal is created
    // Must be initialized to set animation string to ids for better performance
    // set your animation id's using Animator.StringToHash("AnimationName") and use the id
    public override void SetAnimationNamesToIds()
    {
        base.SetAnimationNamesToIds();
        goIdle_id = Animator.StringToHash("goIdle");
        goWalk_id = Animator.StringToHash("goWalk");
    }


    //******* MUST BE FILLED*******//
    // only called once when the animal is created
    // This method takes the states of the animal and initalizes them
    // Initialize method calls this method
    public override void InitializeStates()
    {
        base.InitializeStates();
        idle = new Idle(this, stateMachine);
        walk = new Walk(this, stateMachine);
        consume = new Consume(this, stateMachine);
        stateMachine.Initialize(idle);
    }

}