using UnityEngine;

namespace Enemies.States
{
    public sealed class State_Patrol : State
    {
        Transform[] _patrolPoints;
        int index = 0;
        public override void OnStart(StateMachine stateMachine, Enemies.Core.Enemy enemy)
        {
             _patrolPoints = enemy.patrolPoits.ToArray();

            if(_patrolPoints.Length <= 1) 
                OnExit(stateMachine, enemy, stateMachine.lookAround);

            enemy._agent.enabled = true;
            enemy._agent.speed = enemy.walkSpeed;
            enemy.characterAnimations.PlayAnimation(Common.Names.AnimationNames.movementAnimationName);
            enemy.characterAnimations.SetCharacterSpeed(0.5f);
        }
        public override void OnTick(StateMachine stateMachine, Enemies.Core.Enemy enemy)
        {
            CheckIfTargetsInVision(enemy);
            if (enemy.enemyVision._targetReferance != null) OnExit(stateMachine, enemy, stateMachine.chase);
      
            float distance = Vector3.Distance(enemy.transform.position, _patrolPoints[index].position);
            if (distance > 1f)
            {
                enemy._agent.SetDestination(_patrolPoints[index].position);
            }
            else
            {
                if (index < _patrolPoints.Length - 1) index++;
                else index = 0;
            }

            if (enemy.enemyVision._targetReferance != null) OnExit(stateMachine, enemy, stateMachine.chase);
        }
        void CheckIfTargetsInVision(Enemies.Core.Enemy _enemy) => _enemy.CheckVision();
        public override void OnExit(StateMachine stateMachine, Enemies.Core.Enemy enemy, State state) => stateMachine.ChangeState(state);
    }
}