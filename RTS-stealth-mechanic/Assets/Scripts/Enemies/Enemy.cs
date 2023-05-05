using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Common.Interfaces;

namespace Enemies.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class Enemy : MonoBehaviour, IPatrol, IOutlineOnMouseOver, IBackstabable
    {
        [field: SerializeField] public List<Transform> patrolPoits = new List<Transform>();
        [field: SerializeField] public Outline outline { get; set; }
        [field: SerializeField] public bool isDead { get; set; }
        internal NavMeshAgent _agent { get; private set; }

        internal CharacterAnimations characterAnimations { get; private set; }
        internal EnemyCombatTargetDetection enemyVision { get; private set; }

        [Range(0, 100)] [SerializeField] float _attackRange = 0.5f;
        internal float attackRange { get { return _attackRange; } }

        [Range(0, 100)] [SerializeField] float _walkSpeed = 3;
        internal float walkSpeed { get { return _walkSpeed; } }

        [Range(0,100)]  [SerializeField] float _runSpeed = 5;
        internal float runSpeed { get { return _runSpeed; } }
        
        [Range(0, 100)] [SerializeField] float _attackCooldwon = 1.5f;
        internal float attackCooldown { get { return _attackCooldwon; } }


        [Range(0, 100)] [SerializeField] float _lookAroundDuration = 4;
        internal float lookAroundDuration { get { return _lookAroundDuration; } }

        [Range(0, 100)] [SerializeField] float _attackDamage = 40;
        internal float attackDamage { get { return _attackDamage; } }

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            outline.enabled = false;

            if (TryGetComponent(out CharacterAnimations charAnimations))
            {
                characterAnimations = charAnimations;
            }
            else Tools.Logger.LogWarning($"CharacterAnimations script has not been found", this);


            if (TryGetComponent(out EnemyCombatTargetDetection enemyVis))
            {
                enemyVision = enemyVis;
            }
            else Tools.Logger.LogWarning($"EnemyVision script has not been found", this);
        }
        internal void CheckVision() => enemyVision.Tick();

        public void ToggleOutline(bool enable)
        {
            if (enable == true) outline.enabled = true;
            else outline.enabled = false;
        }

        public void GetStabed(Transform stabbedEnemy)
        {
            if (stabbedEnemy == transform)
            {
                PerformDeath();
            }
        }
        void PerformDeath()
        {
            _agent.enabled = false;
            if (TryGetComponent(out Enemies.States.StateMachine state))
            {
                state.enabled = false;
            }
            characterAnimations.PlayAnimation(Common.Names.AnimationNames.deathAnimationName);
            isDead = true;
            this.enabled = false;

        }
        public void OnEnable()
        {
            Events.GameEvents.OnPlayerAssassinationPerformedNotifyTarget.AddListener(GetStabed);
        }
        public void OnDisable()
        {
            Events.GameEvents.OnPlayerAssassinationPerformedNotifyTarget.RemoveListener(GetStabed);
        }
    }
}