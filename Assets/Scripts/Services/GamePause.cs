using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Services {
    public class GamePause : UIService {
        [SerializeField, Scene] private string mainMenu = null!;

        void Start() {
            var root = document.rootVisualElement;
            var resume = root.Q<Button>("resume");
            resume.Focus();
            resume.clicked += Resume;
            root.Q<Button>("options").clicked += ShowOptions;
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

        private void ShowOptions() {
            document.rootVisualElement.SetEnabled(false);
            Options.Instance.Show(() => document.rootVisualElement.SetEnabled(true));
        }

        private void LoadMainMenu() {
            Resume();
            SceneManager.LoadScene(mainMenu);
        }
    }
}
