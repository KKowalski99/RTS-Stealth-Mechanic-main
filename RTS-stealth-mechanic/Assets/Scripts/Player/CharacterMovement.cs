using UnityEngine;
using UnityEngine.AI;


namespace PlayerCharacter.Core
{
    [RequireComponent(typeof(NavMeshAgent))]
    public sealed class CharacterMovement : MonoBehaviour, Common.Interfaces.IEventListenable
    {
        Actions _actions;
        [Tooltip("Connect InputHandler from InputHandler Script")] [SerializeField] InputActionsHolder _inputHandler;
        [SerializeField] Camera _mainCamera;
        [Tooltip("Debug object moved to player target move positon on click")] [SerializeField] Transform _destinationPointer;
        [SerializeField] bool _isDestinationPointerActive = true;
        [Tooltip("Script responsible for playing animations")] CharacterAnimations _characterAnimations;

        NavMeshAgent _navmeshAgent;

        Common.Enums.MovementType _movementType;

        [Range(0, 100)] [SerializeField] float _runSpeed;
        [Range(0, 100)] [SerializeField] float _crouchSpeed;

        private void Awake()
        {
            _actions = _inputHandler._actions;

            _navmeshAgent = GetComponent<NavMeshAgent>();
            _navmeshAgent.enabled = false;
            _destinationPointer.transform.position = transform.position;

            _actions.GameMode.MouseClick.performed += ctx => MouseClick();

            if (TryGetComponent(out CharacterAnimations characterAnimations))
            {
                _characterAnimations = characterAnimations;
            }
            _movementType = Common.Enums.MovementType.Run;

            if (_isDestinationPointerActive == false) _destinationPointer.gameObject.SetActive(false);
            else _destinationPointer.gameObject.SetActive(true);

            if (_runSpeed == 0) Tools.Logger.LogWarning("_runSpeed variable is set to 0", this);
            if (_crouchSpeed == 0) Tools.Logger.LogWarning("_crouchSpeed variable is set to 0", this);
            if (_actions == null)
            {
                Tools.Logger.LogError("_actions referance is missing", this);
                return;
            }
        }
        void MouseClick()
        {
            Vector2 mousePosition = _actions.GameMode.MousePosition.ReadValue<Vector2>();
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (hit.transform.CompareTag(Common.Names.GameTags.groundTag)) _destinationPointer.position = hit.point;
            }
        }
        private void Update()
        {
            if (Vector3.Distance(transform.position, _destinationPointer.position) > 0.4f)
            {
                _navmeshAgent.enabled = true;
                _navmeshAgent.SetDestination(_destinationPointer.position);
                if (_characterAnimations != null)
                {
                    if (_movementType == Common.Enums.MovementType.Crouch)
                    {
                        _navmeshAgent.speed = _crouchSpeed;
                        _characterAnimations.SetCharacterSpeed(1.5f);
                    }
                    else
                    {
                        _navmeshAgent.speed = _runSpeed;
                        _characterAnimations.SetCharacterSpeed(1f);
                    }
                }
            }
            else
            {
                if (_characterAnimations != null)
                {
                    if (_movementType == Common.Enums.MovementType.Crouch) _characterAnimations.SetCharacterSpeed(0);
                    else _characterAnimations.SetCharacterSpeed(0.5f);
                }
                _navmeshAgent.enabled = false;
            }
        }
        void ToggleMovementType(bool isCrouchActive)
        {
            if (isCrouchActive == false) _movementType = Common.Enums.MovementType.Run;
            else _movementType = Common.Enums.MovementType.Crouch;
        }
        void ResetMovementDirectionOnOtherAction()
        {
            _navmeshAgent.enabled = false;
            _destinationPointer.position = transform.position;
        }
        public void OnEnable()
        {
            Events.GameEvents.OnToggleCrouchMovementMode.AddListener(ToggleMovementType);
            Events.GameEvents.OnAssassinationPerformed.AddListener(ResetMovementDirectionOnOtherAction);
        }
        public void OnDisable()
        {
            Events.GameEvents.OnToggleCrouchMovementMode.RemoveListener(ToggleMovementType);
            Events.GameEvents.OnAssassinationPerformed.RemoveListener(ResetMovementDirectionOnOtherAction);
        }
    }
}