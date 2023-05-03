using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour, Common.Interfaces.IEventListenable
    {
        [SerializeField] Image _skillStealthGrayOut;
        [SerializeField] Image _screenStealthGrayOut;
        [SerializeField] Image _skillAssasinationGrayOut;

        void ToggleStealthSkillGrayOut(bool isSkillActive)
        {
            if (isSkillActive == true)
            {
                _skillStealthGrayOut.enabled = true;
                _screenStealthGrayOut.enabled = true;
            }
            else
            {
                _skillStealthGrayOut.enabled = false;
                _screenStealthGrayOut.enabled = false;
            }
        }
        void ToggleAssassinationActionGrayOut(bool isSkillActive)
        {
            if (isSkillActive == true) _skillAssasinationGrayOut.enabled = false;
            else _skillAssasinationGrayOut.enabled = true;
        }
        public void OnDisable()
        {
            Events.GameEvents.OnToggleCrouchMovementMode.RemoveListener(ToggleStealthSkillGrayOut);
            Events.GameEvents.OnToggleAssasinationCombatAction.RemoveListener(ToggleAssassinationActionGrayOut);
        }
        public void OnEnable()
        {
            Events.GameEvents.OnToggleCrouchMovementMode.AddListener(ToggleStealthSkillGrayOut);
            Events.GameEvents.OnToggleAssasinationCombatAction.AddListener(ToggleAssassinationActionGrayOut);
        }
    }
}