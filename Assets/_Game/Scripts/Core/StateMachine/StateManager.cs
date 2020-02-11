namespace Ibit.Core.StateMachine
{
    public class StateManager
    {
        private IState currentState;
        private IState previousState;

        public void ChangeState(IState newState)
        {
            currentState?.Exit();
            this.previousState = this.currentState;
            this.currentState = newState;
            this.currentState.Enter();
        }

        public void ExecuteStateUpdate()
        {
            var runningState = this.currentState;
            runningState?.Execute();
        }

        public void SwitchToPreviousState()
        {
            currentState.Exit();
            currentState = previousState;
            currentState.Enter();
        }
    }
}