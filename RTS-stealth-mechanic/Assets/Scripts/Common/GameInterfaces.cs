using System.Collections.Generic;
using UnityEngine;

namespace Common.Interfaces
{
    public interface IBackstabable
    {
        [field: SerializeField] public bool isDead { get; set; }
        public void GetStabed(Transform target);
    }
    public interface IEventListenable
    {
        public void OnEnable();
        public void OnDisable();
    }

    public interface IOutlineOnMouseOver
    {
        [field: SerializeField] public Outline outline { get; set; }
        public void ToggleOutline(bool enable);

    }
    public interface IPatrol
    {
        [field: SerializeField] public List<Transform> patrolPoits { get { return patrolPoits; } set { patrolPoits = new List<Transform>(); } }
    }

    public interface IEnemyTarget { }
    public interface IDamageable
    {
        public void GetDamage(float damage);
    }

    public interface IHearable 
    {
        [field: SerializeField] public float minNoiseLevelDetection { get; set; }
        public void IsTargetDetected(float noiseLevel, Transform target);
    }
  
}