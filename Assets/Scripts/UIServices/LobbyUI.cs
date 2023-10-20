using System.Collections.Generic;
using KitchenChaos.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class LobbyUI : UIService {
        public static string PlayerName {
            get => PlayerPrefs.GetString("PlayerName");
            private set => PlayerPrefs.SetString("PlayerName", value);
        }

        private readonly List<Button> lobbyButtons = new();

        void Start() {
            var root = document.rootVisualElement;
            root.Q<Button>("main-menu").clicked += SceneService.Instance.LoadMainMenu;
            var playerName = root.Q<TextField>("player-name");
            playerName.RegisterValueChangedCallback(OnNameChanged);
            var create = root.Q<Button>("create");
            create.Focus();
            create.clicked += Create;
            root.Q<Button>("join").clicked += NetworkService.Instance.StartClient;
            LobbyButton("create-lobby").clicked += CreateLobby.Instance.Show;
            LobbyButton("lobbies").clicked += Lobbies.Instance.Show;
            LobbyButton("quick-join").clicked += NetworkLobby.Instance.QuickJoin;
            LobbyButton("code-join").clicked += CodeJoin;
            SetLobbyButtonsEnabled(playerName.value = PlayerName);
            return;

            void Create() {
                NetworkService.Instance.StartHost();
                SceneService.Instance.LoadCharacterSelection();
            }

            void OnNameChanged(ChangeEvent<string> evt) {
                SetLobbyButtonsEnabled(evt.newValue);
                PlayerName = evt.newValue;
            }

            void CodeJoin() {
                NetworkLobby.Instance.CodeJoin(root.Q<TextField>("join-code").value);
            }
        }

        private Button LobbyButton(string id) {
            var button = document.rootVisualElement.Q<Button>(id);
            lobbyButtons.Add(button);
            return button;
        }

        private void SetLobbyButtonsEnabled(string playerName) {
            var empty = string.IsNullOrEmpty(playerName);
            foreach (var button in lobbyButtons) {
                button.SetEnabled(!empty);
            }
        }
    }
}
