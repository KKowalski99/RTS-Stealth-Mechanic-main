using UnityEngine;

namespace Events
{
    public static class GameEvents
    {
        public static readonly Event<bool> OnToggleCrouchMovementMode = new Event<bool>();
        public static readonly Event<bool> OnToggleAssasinationCombatAction = new Event<bool>();

        public static readonly Event OnAssassinationPerformed = new Event();
        public static readonly Event<string> OnPlayerAssassinationPerformedPlayAnimation = new Event<string>();
        public static readonly Event<Transform> OnPlayerAssassinationPerformedNotifyTarget = new Event<Transform>();

        public static readonly Event OnPlayerDeath = new Event();
        public static readonly Event OnGameRestart = new Event(); 
    }
}