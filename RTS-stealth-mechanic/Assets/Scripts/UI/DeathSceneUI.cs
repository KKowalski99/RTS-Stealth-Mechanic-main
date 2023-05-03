using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class DeathSceneUI : MonoBehaviour
    {
        [SerializeField] Button _restardButton;
        [SerializeField] Button _quitButton;
        void Start()
        {
            _restardButton.onClick.AddListener(RestardGame);
            _quitButton.onClick.AddListener(QuitGame);
        }
        void RestardGame() => Events.GameEvents.OnGameRestart.Invoke();
        void QuitGame() => Application.Quit();
    }
}
