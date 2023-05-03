using UnityEngine;

namespace PlayerCharacter.Core
{
    public sealed class PlayerCombatActions : MonoBehaviour, Common.Interfaces.IEventListenable
    {
        Transform _enemyInRange = null;
        [SerializeField] float _enemyDetectionRadius = 10f;
        [Range(0, 360)] [SerializeField] float _detectionAngle = 120;
        bool _stealthActive;

        Actions _actions;
        [SerializeField] InputHandler _inputHandler;

        [SerializeField] LayerMask _interactionMask;
        private void Start() => _actions = _inputHandler._actions;


        private void Update()
        {
            StealthAssasinationAvailable();
            StealthMode();
        }

        #region StealthMode
        void StealthMode()
        {
            bool isStealthModeToggled = _actions.GameMode.Stealth.WasReleasedThisFrame();
            if (isStealthModeToggled)
            {
                if (_stealthActive)
                {
                    Events.GameEvents.OnToggleCrouchMovementMode.Invoke(false);
                    _stealthActive = false;
                }
                else
                {
                    Events.GameEvents.OnToggleCrouchMovementMode.Invoke(true);
                    _stealthActive = true;
                }
            }
        }
        #endregion

        #region
        void StealthAssasinationAvailable()
        {
            if (_stealthActive == false)
            {
                Events.GameEvents.OnToggleAssasinationCombatAction.Invoke(false);
                return;
            }
            bool isInPossitionToBackstab = false;

            Collider[] rangeCheck = Physics.OverlapSphere(transform.position, _detectionAngle, _interactionMask);

            Vector3 directionFromTargetToOwner = Vector3.zero;


            if (rangeCheck.Length > 0)
            {
                foreach (Collider collider in rangeCheck)
                {
                    if (collider.GetComponent<Common.Interfaces.IBackstabable>() != null && collider.GetComponent<Common.Interfaces.IBackstabable>().isDead != true)
                    {

                        _enemyInRange = collider.transform;
                    }
                }
                if (_enemyInRange != null)
                {
                    directionFromTargetToOwner = (transform.position - _enemyInRange.position).normalized;

                    float distance = Vector3.Distance(_enemyInRange.position, transform.position);
                    float dotValue = Vector3.Dot(_enemyInRange.forward, directionFromTargetToOwner);

                    if (distance < _enemyDetectionRadius && dotValue < -1 + 0.45f) isInPossitionToBackstab = true;
                }
            }
            bool isAssassinationKeyPresed = _actions.GameMode.Assassination.WasPressedThisFrame();
            if (isInPossitionToBackstab == true)
            {
                Events.GameEvents.OnToggleAssasinationCombatAction.Invoke(true);
                if (isAssassinationKeyPresed) PerformAssassination();
            }
            else Events.GameEvents.OnToggleAssasinationCombatAction.Invoke(false);
        }
        void PerformAssassination()
        {
            Vector3 dir = (_enemyInRange.position - transform.position).normalized;
            transform.LookAt(dir);
            Events.GameEvents.OnAssassinationPerformed.Invoke();
            Events.GameEvents.OnPlayerAssassinationPerformedPlayAnimation.Invoke(Common.Names.AnimationNames.assassinationAnimationName);
            Events.GameEvents.OnPlayerAssassinationPerformedNotifyTarget.Invoke(_enemyInRange);
            _enemyInRange = null;
        }

        #endregion
        void StealthModeActiveInfo(bool isActive) => _stealthActive = isActive;
        public void OnEnable()
        {
            Events.GameEvents.OnToggleCrouchMovementMode.AddListener(StealthModeActiveInfo);
        }
        public void OnDisable()
        {
            Events.GameEvents.OnToggleCrouchMovementMode.RemoveListener(StealthModeActiveInfo);
        }
    }
}
