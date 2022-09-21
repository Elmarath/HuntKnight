public abstract class State
{
    protected Animal animal;
    protected StateMachine stateMachine;

    protected State(Animal rabbit, StateMachine stateMachine)
    {
        this.animal = rabbit;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void HandleInput()
    {
        // whenever we exit the state stop coroutines
    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void Exit()
    {

    }

}