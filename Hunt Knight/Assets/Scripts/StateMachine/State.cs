public abstract class State
{
    protected Animal animal;
    protected StateMachine stateMachine;

    protected State(Animal animal, StateMachine stateMachine)
    {
        this.animal = animal;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void HandleInput()
    {
        // whenever we exit the state stop coroutines
        HandleInterrupts();
    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void Exit()
    {

    }

    public virtual void HandleInterrupts()
    {

    }

}