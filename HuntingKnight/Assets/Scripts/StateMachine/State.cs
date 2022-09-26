public abstract class State
{
    protected CommonAnimal commonAnimal;
    protected StateMachine stateMachine;

    protected State(CommonAnimal commonAnimal, StateMachine stateMachine)
    {
        this.commonAnimal = commonAnimal;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        commonAnimal.isStateFinished = false;
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
        commonAnimal.isStateFinished = true;
    }

    public virtual void HandleInterrupt()
    {
        commonAnimal.HandleInterrupt();
    }

}