using UnityEngine;

namespace Enemies.States
{
    public sealed class StateMachine : MonoBehaviour
    {
        [SerializeField] Enemies.Core.Enemy enemy;
        State _state;
        public readonly State_Patrol patrol = new State_Patrol();
        public readonly State_Chase chase = new State_Chase();
        public readonly State_Attack attack = new State_Attack();
        public readonly State_LookAround lookAround = new State_LookAround();
        private void Start() => ChangeState(patrol);
        public void ChangeState(State state)
        {
            _state = state;
            _state.OnStart(this, enemy);
            Debug.Log(state);
        }
        private void Update() => _state?.OnTick(this, enemy);
    }
}