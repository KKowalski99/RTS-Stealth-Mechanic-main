using UnityEngine;

namespace PlayerCharacter.Core
{
    public sealed class Player : MonoBehaviour, Common.Interfaces.IOutlineOnMouseOver, Common.Interfaces.IEnemyTarget, Common.Interfaces.IDamageable
    {
       [Range(0,100)] [SerializeField] float _health = 100;
        public Outline outlinbe { get; set; }

        void Awake() 
        {
            if (_health <= 0) Tools.Logger.LogError("player health is set to 0", this);
        }

        void Start()
        {
            if (outlinbe == null)
            {
                if (TryGetComponent(out Outline temp))
                {
                    outlinbe = temp;
                }
            }
            outlinbe.enabled = false;
        }

        public void EnableOutline() => outlinbe.enabled = true;
        public void DisableOutline() => outlinbe.enabled = false;

        public void GetDamage(float damage)
        {
                _health -= damage;
                if (_health <= 0) PlayerDeath();
        }


        void PlayerDeath()
        {
            Events.GameEvents.OnPlayerDeath.Invoke();
        }
    
    }
}