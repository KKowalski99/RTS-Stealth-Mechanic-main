using UnityEngine;

namespace Enemies.States
{
    public sealed class State_LookAround : State
    {
        float time;
        public override void OnStart(StateMachine stateMachine, Enemies.Core.Enemy enemy)
        {
            enemy._agent.enabled = false;
            enemy.characterAnimations.PlayAnimation(Common.Names.AnimationNames.lookAroundAnimationName);
            
        }
        public override void OnTick(StateMachine stateMachine, Enemies.Core.Enemy enemy)
        {
            time += Time.deltaTime;

            CheckIfTargetsInVision(enemy);
            Transform _target = enemy.enemyVision._targetReferance;

            if (_target != null) OnExit(stateMachine, enemy, stateMachine.chase);
        
            if(time > enemy.lookAroundDuration) 
                OnExit(stateMachine, enemy, stateMachine.patrol);
        }
        void CheckIfTargetsInVision(Enemies.Core.Enemy _enemy) => _enemy.CheckVision();
        public override void OnExit(StateMachine stateMachine, Enemies.Core.Enemy enemy, State state)
        {
            time = 0;
            enemy._agent.enabled = true;
            stateMachine.ChangeState(state);
        }
    }
}