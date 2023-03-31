using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Services {
    public class GamePause : UIService {
        [NaughtyAttributes.Scene, SerializeField]
        private string mainMenu = null!;

        void Start() {
            var root = document.rootVisualElement;
            root.Q<Button>("resume").clicked += Resume;
            root.Q<Button>("menu").clicked += LoadMainMenu;
            root.SetActive(false);
            GameService.Instance.Paused += Show;
            GameService.Instance.Unpaused += Hide;

            void Show() => root.SetActive(true);
            void Hide() => root.SetActive(false);
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
