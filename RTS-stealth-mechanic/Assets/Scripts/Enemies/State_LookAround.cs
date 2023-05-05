using UnityEngine;
using Enemies.Core;

namespace Enemies.States
{
    public sealed class State_LookAround : State
    {
        float time;
        public override void OnStart(StateMachine stateMachine, Enemy enemy)
        {
            enemy._agent.enabled = false;
            enemy.characterAnimations.PlayAnimation(Common.Names.AnimationNames.lookAroundAnimationName);
            
        }
        public override void OnTick(StateMachine stateMachine, Enemy enemy)
        {
            time += Time.deltaTime;

            CheckIfTargetsInVision(enemy);
            Transform _target = enemy.enemyVision._targetReferance;

            if (_target != null) OnExit(stateMachine, enemy, stateMachine.chase);
        
            if(time > enemy.lookAroundDuration) 
                OnExit(stateMachine, enemy, stateMachine.patrol);
        }
        void CheckIfTargetsInVision(Enemy _enemy) => _enemy.CheckVision();
        public override void OnExit(StateMachine stateMachine, Enemy enemy, State state)
        {
            time = 0;
            enemy._agent.enabled = true;
            stateMachine.ChangeState(state);
        }
    }
}