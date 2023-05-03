namespace Enemies.States
{
   public abstract class State
    {
        public abstract void OnStart(StateMachine stateMachine, Enemies.Core.Enemy enemy);
        public abstract void OnTick(StateMachine stateMachine, Enemies.Core.Enemy enemy);
        public abstract void OnExit(StateMachine stateMachine, Enemies.Core.Enemy enemy, State state);
    }
}