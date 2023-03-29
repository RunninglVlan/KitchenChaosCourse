using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Services {
    public class MainMenu : UIService {
        [NaughtyAttributes.Scene, SerializeField]
        private string loadingScene = null!;

        [NaughtyAttributes.Scene, SerializeField]
        private string gameScene = null!;

        void Start() {
            document.rootVisualElement.Q<Button>("play").clicked += LoadGameScene;
            document.rootVisualElement.Q<Button>("quit").clicked += Application.Quit;
        }

        private void LoadGameScene() {
            SceneManager.LoadSceneAsync(loadingScene).completed += _ => {
                SceneManager.LoadScene(gameScene);
            };
        }
    }
}
