﻿using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class GamePause : UIService {
        void Start() {
            var root = document.rootVisualElement;
            var resume = root.Q<Button>("resume");
            resume.Focus();
            resume.clicked += Resume;
            root.Q<Button>("options").clicked += ShowOptions;
            root.Q<Button>("menu").clicked += LoadMainMenu;
            Hide();
            GameService.Instance.LocalPaused += Show;
            GameService.Instance.LocalUnpaused += Hide;
        }

        private static void Resume() {
            GameService.Instance.TogglePause();
        }

        private void ShowOptions() {
            Hide();
            Options.Instance.Show(Show);
        }

        private static void LoadMainMenu() {
            Resume();
            Time.timeScale = 1;
            if (!NetworkManager.Singleton.IsServer) {
                NetworkManager.Singleton.Shutdown();
            }
            SceneService.Instance.LoadMainMenu();
        }
    }
}
