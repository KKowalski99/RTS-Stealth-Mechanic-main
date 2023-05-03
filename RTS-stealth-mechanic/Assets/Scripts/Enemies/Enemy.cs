using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class Enemy : MonoBehaviour, Common.Interfaces.IPatrol, Common.Interfaces.IOutlineOnMouseOver, Common.Interfaces.IBackstabable
    {
        [field: SerializeField] public List<Transform> patrolPoits = new List<Transform>();
        [field: SerializeField] public Outline outlinbe { get; set; }
        [field: SerializeField] public bool isDead { get; set; }
        public NavMeshAgent _agent { get; private set; }

        public CharacterAnimations characterAnimations { get; private set; }
        public EnemyCombatTargetDetection enemyVision { get; private set; }

        [Range(0, 100)] [SerializeField] float _attackRange = 0.5f;
        public float attackRange { get { return _attackRange; } }

        [Range(0, 100)] [SerializeField] float _walkSpeed = 3;
        public float walkSpeed { get { return _walkSpeed; } }

        [Range(0,100)]  [SerializeField] float _runSpeed = 5;
        public float runSpeed { get { return _runSpeed; } }
        
        [Range(0, 100)] [SerializeField] float _attackCooldwon = 1.5f;
        public float attackCooldown { get { return _attackCooldwon; } }


        [Range(0, 100)] [SerializeField] float _lookAroundDuration = 4;
        public float lookAroundDuration { get { return _lookAroundDuration; } }

        [Range(0, 100)] [SerializeField] float _attackDamage = 40;
        public float attackDamage { get { return _attackDamage; } }


        public void DisableOutline() => outlinbe.enabled = false;
        public void EnableOutline() => outlinbe.enabled = true;

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            outlinbe.enabled = false;

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
        public void CheckVision() => enemyVision.Tick();
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