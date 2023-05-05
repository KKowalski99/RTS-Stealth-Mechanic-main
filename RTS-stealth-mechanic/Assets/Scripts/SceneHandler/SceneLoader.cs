using UnityEngine;
using UnityEngine.SceneManagement;
using Common.Names;

namespace SceneHandler
{
    public sealed class SceneLoader : MonoBehaviour, Common.Interfaces.IEventListenable
    {
        public static SceneLoader Instance { get; private set; }
        private Scene scene;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Tools.Logger.LogWarning($"SceneManager duplicated, game object has been destroyed", this);
                Destroy(gameObject);
            }
            scene = SceneManager.GetActiveScene();
        }

        private void Start()
        {
            if (scene.name == SceneNames.coreSceneName)
            {
                if (SceneManager.GetSceneByName(SceneNames.uiSceneName).isLoaded == false)
                    SceneManager.LoadSceneAsync(SceneNames.uiSceneName, mode: LoadSceneMode.Additive);

                if (SceneManager.GetSceneByName(SceneNames.environmentSceneName).isLoaded == false)
                    SceneManager.LoadSceneAsync(SceneNames.environmentSceneName, mode: LoadSceneMode.Additive);
            }
        }
         void Restart() => SceneManager.LoadScene(SceneNames.coreSceneName);
         void LoadDeathScene() => SceneManager.LoadScene(SceneNames.deathSceneName);

        public void OnEnable()
        {
            Events.GameEvents.OnPlayerDeath.AddListener(LoadDeathScene);
            Events.GameEvents.OnGameRestart.AddListener(Restart);
        }

        public void OnDisable()
        {
            Events.GameEvents.OnPlayerDeath.RemoveListener(LoadDeathScene);
            Events.GameEvents.OnGameRestart.RemoveListener(Restart);
        }
    }
}