using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KitchenChaos.Services {
    public class SceneService : MonoSingleton<SceneService> {
        [SerializeField, Scene] private string mainMenu = null!;
        [SerializeField, Scene] private string loading = null!;
        [SerializeField, Scene] private string game = null!;

        public void LoadGame() {
            SceneManager.LoadSceneAsync(loading).completed += _ => {
                SceneManager.LoadScene(game);
            };
        }

        public void LoadMainMenu() => SceneManager.LoadScene(mainMenu);
    }
}
