using UnityEngine;
using Common.Interfaces;

namespace PlayerCharacter.Core
{
    public sealed class Player : MonoBehaviour, IOutlineOnMouseOver, IEnemyTarget, IDamageable
    {
       [Range(0,100)] [SerializeField] float _health = 100;
        public Outline outline { get; set; }

        void Awake() 
        {
            if (_health <= 0) Tools.Logger.LogError("player health is set to 0 or below", this);
        }

        void Start()
        {
            if (outline == null)
            {
                if (TryGetComponent(out Outline temp))
                {
                    outline = temp;
                }
            }
            outline.enabled = false;
        }
        public void ToggleOutline(bool enable)
        {
            if (enable == true) outline.enabled = true;
            else outline.enabled = false;
        }

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