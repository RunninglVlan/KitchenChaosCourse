using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Services {
    public class GamePause : UIService {
        [NaughtyAttributes.Scene, SerializeField]
        private string mainMenu = null!;

        void Start() {
            document.rootVisualElement.Q<Button>("resume").clicked += Resume;
            document.rootVisualElement.Q<Button>("menu").clicked += LoadMainMenu;
            document.rootVisualElement.SetActive(false);
            GameService.Instance.StateChanged += SetActive;
        }

        private void SetActive() {
            document.rootVisualElement.SetActive(GameService.Instance.IsGamePaused);
        }

        private static void Resume() {
            GameService.Instance.TogglePause();
        }

        private void LoadMainMenu() {
            Resume();
            SceneManager.LoadScene(mainMenu);
        }
    }
}
