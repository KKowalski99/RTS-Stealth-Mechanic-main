using Enemies.Core;

namespace Enemies.States
{
   public abstract class State
    {
        public abstract void OnStart(StateMachine stateMachine, Enemy enemy);
        public abstract void OnTick(StateMachine stateMachine, Enemy enemy);
        public abstract void OnExit(StateMachine stateMachine, Enemy enemy, State state);
    }
}