using UnityEngine;
using Enemies.Core;
using Common.Names;

namespace Enemies.States
{
    public sealed class State_Chase : State
    {
        Transform _target;
        Vector3 _lastKnownTargetPosition;
        bool _followLastKnownPositionAfterTargetOutOfRange;
        public override void OnStart(StateMachine stateMachine, Enemy enemy)
        {
            _target = enemy.enemyVision._targetReferance;
            enemy._agent.enabled = true;
            enemy.characterAnimations.PlayAnimation(AnimationNames.movementAnimationName);
            enemy._agent.speed = enemy.runSpeed;
            enemy.characterAnimations.SetCharacterSpeed(1f);
        }
        public override void OnTick(StateMachine stateMachine, Enemy enemy)
        {
            CheckIfTargetsInVision(enemy);
            _target = enemy.enemyVision._targetReferance;

            if (_target != null)
            {
                _lastKnownTargetPosition = _target.position;
                float distance = Vector3.Distance(enemy.transform.position, _target.position);
                _followLastKnownPositionAfterTargetOutOfRange = true;

                if (distance > 1.5f) enemy._agent.SetDestination(_target.position); 
               else OnExit(stateMachine, enemy, stateMachine.attack);
            }
            else if (_target == null && _followLastKnownPositionAfterTargetOutOfRange == true)
            {
                float distance = Vector3.Distance(enemy.transform.position, _lastKnownTargetPosition);

                if (distance > 0.4) enemy._agent.SetDestination(_lastKnownTargetPosition);
                else _followLastKnownPositionAfterTargetOutOfRange = false;
            }
            else OnExit(stateMachine, enemy, stateMachine.lookAround);
        }
        void CheckIfTargetsInVision(Enemy _enemy) => _enemy.CheckVision();
        public override void OnExit(StateMachine stateMachine, Enemy enemy, State state) => stateMachine.ChangeState(state);
    }
}
