using UnityEngine;

// This is a base class for all animals
// copy this to your wanted animal class and change to your needs
public class BaseExampleAnimalScript : Herbivore // It's either Herbivore or Carnivorous
{
    // external Animal attributes are set here
    #region AnimalAttributes
    #endregion

    // Field of view attributes filled here
    #region FieldOfView
    #endregion

    // Which states the animal has
    #region StateVeriables
    [HideInInspector]
    public Idle idle;
    [HideInInspector]
    public Walk walk;
    #endregion

    // Control Variables for state machine
    #region stateChecks
    [HideInInspector]
    public bool goIdle = false;
    [HideInInspector]
    public bool goWalk = false;
    #endregion

    // This variables are to be converted to ids for better performance
    #region animationStringIds
    private int goIdle_id;
    private int goWalk_id;
    #endregion

    // additional state checks for rabbit
    #region stateChecks

    #endregion

    // Instant state checks for animal
    #region instantStateChecks

    #endregion

    void Awake()
    {
        Initialize();
    }

    //******* MUST BE FILLED*******//
    // This method is used to update the animation variables of the animal
    public override void UpdateAnimationVariables()
    {
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
        // idle = new Idle(this, stateMachine);
        // walk = new Walk(this, stateMachine);
        stateMachine.Initialize(idle);
    }
}



