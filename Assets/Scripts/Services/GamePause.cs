using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace KitchenChaos.Services {
    public class GamePause : UIService {
        [SerializeField, Scene] private string mainMenu = null!;

        void Start() {
            var root = document.rootVisualElement;
            var resume = root.Q<Button>("resume");
            resume.Focus();
            resume.clicked += Resume;
            root.Q<Button>("options").clicked += ShowOptions;
            root.Q<Button>("menu").clicked += LoadMainMenu;
            Hide();
            GameService.Instance.Paused += Show;
            GameService.Instance.Unpaused += Hide;
        }

        private static void Resume() {
            GameService.Instance.TogglePause();
        }

        private void ShowOptions() {
            Hide();
            Options.Instance.Show(Show);
        }

        private void LoadMainMenu() {
            Resume();
            SceneManager.LoadScene(mainMenu);
        }
    }
}
