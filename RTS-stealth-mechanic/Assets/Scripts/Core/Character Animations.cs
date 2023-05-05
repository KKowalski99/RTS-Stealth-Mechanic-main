using UnityEngine;
using Common.Names;

[RequireComponent(typeof(Animator))]
public sealed class CharacterAnimations : MonoBehaviour, Common.Interfaces.IEventListenable
{
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void SetCharacterSpeed(float speed)
    {
        if (speed < 0) return;
        anim.SetFloat(AnimationNames.variableNameSpeed, speed);
    }
    public void PlayAnimation(string animationName)
    {
        anim?.Play(animationName);   
    }
    public void OnEnable()
    {
        Events.GameEvents.OnPlayerAssassinationPerformedPlayAnimation.AddListener(PlayAnimation);
    }
    public void OnDisable()
    {
        Events.GameEvents.OnPlayerAssassinationPerformedPlayAnimation.RemoveListener(PlayAnimation);
    }

}
