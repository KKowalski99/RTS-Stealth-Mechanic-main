using UnityEngine;
using Enemies.Core;

namespace Enemies.States
{
    public sealed class State_Attack : State
    {
        Transform target;
        Transform targetLastKnownPosition;
        const string _slashAnimName = "Attack_Slash";

        bool isPerformingAttack;

        float time;
        public override void OnStart(StateMachine stateMachine, Enemy enemy)
        {
            enemy._agent.enabled = false;
            time = enemy.attackCooldown;
        }

        public override void OnTick(StateMachine stateMachine, Enemy enemy)
        {
            CheckIfTargetsInVision(enemy);
            target = enemy.enemyVision._targetReferance;

            if (target != null)
            {
                targetLastKnownPosition = target;
                if (isPerformingAttack == false)
                {
                    float distance = Vector3.Distance(enemy.transform.position, target.position);
                    if (distance <= enemy.attackRange + 0.2f)
                    {
                        enemy.transform.LookAt(target);
                        enemy.characterAnimations.PlayAnimation(_slashAnimName);
                       target.GetComponent<Common.Interfaces.IDamageable>()?.GetDamage(enemy.attackDamage);
                       isPerformingAttack = true;
                    }
                    else OnExit(stateMachine, enemy, stateMachine.chase);
                
                }
                else AttackCooldown(enemy);
                
            }
            else
            {
                enemy.transform?.LookAt(targetLastKnownPosition);
                OnExit(stateMachine, enemy, stateMachine.lookAround);
            }
        }
        void AttackCooldown(Enemy enemy)
        {
            if(isPerformingAttack)
            {
                time -= Time.deltaTime;
                if(time <= 0)
                {
                    isPerformingAttack = false;
                    time = enemy.attackCooldown;
                }
            }
        }
        void CheckIfTargetsInVision(Enemy _enemy)
        {
            _enemy.CheckVision();
        }
        public override void OnExit(StateMachine stateMachine, Enemy enemy, State state)
        {
            stateMachine.ChangeState(state);
        }

    }
}