namespace Ibit.Core.StateMachine
{
    public interface IState
    {
        void Enter();

        void Execute();

        void Exit();
    }
}