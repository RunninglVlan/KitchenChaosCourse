using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Services {
    public class MainMenu : UIService {
        [SerializeField, Scene] private string loadingScene = null!;
        [SerializeField, Scene] private string gameScene = null!;

        void Start() {
            var play = document.rootVisualElement.Q<Button>("play");
            play.Focus();
            play.clicked += LoadGameScene;
            document.rootVisualElement.Q<Button>("quit").clicked += Application.Quit;
        }

        private void LoadGameScene() {
            SceneManager.LoadSceneAsync(loadingScene).completed += _ => {
                SceneManager.LoadScene(gameScene);
            };
        }
    }
}
