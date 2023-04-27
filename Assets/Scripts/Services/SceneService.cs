using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KitchenChaos.Services {
    public class SceneService : MonoSingleton<SceneService> {
        [SerializeField, Scene] private string mainMenuScene = null!;
        [SerializeField, Scene] private string loadingScene = null!;
        [SerializeField, Scene] private string gameScene = null!;

        public void LoadGame() {
            SceneManager.LoadSceneAsync(loadingScene).completed += _ => {
                SceneManager.LoadScene(gameScene);
            };
        }

        public void LoadMainMenu() => SceneManager.LoadScene(mainMenuScene);
    }
}
