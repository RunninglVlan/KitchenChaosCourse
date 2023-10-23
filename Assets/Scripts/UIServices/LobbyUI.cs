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

        private readonly List<Button> multiplayerButtons = new();

        void Start() {
            var root = document.rootVisualElement;
            root.Q<Button>("main-menu").clicked += SceneService.Instance.LoadMainMenu;
            var playerName = root.Q<TextField>("player-name");
            playerName.RegisterValueChangedCallback(OnNameChanged);
            var create = root.Q<Button>("create");
            create.Focus();
            create.clicked += Create;
            root.Q<Button>("join").clicked += NetworkService.Instance.StartClient;
            MultiplayerButton("create-lobby").clicked += CreateLobby.Instance.Show;
            MultiplayerButton("lobbies").clicked += Lobbies.Instance.Show;
            MultiplayerButton("quick-join").clicked += NetworkLobby.Instance.QuickJoin;
            MultiplayerButton("code-join").clicked += CodeJoin;
            SetMultiplayerButtonsEnabled(playerName.value = PlayerName);
            return;

            void Create() {
                NetworkService.Instance.StartHost();
                SceneService.Instance.LoadCharacterSelection();
            }

            void OnNameChanged(ChangeEvent<string> evt) {
                SetMultiplayerButtonsEnabled(evt.newValue);
                PlayerName = evt.newValue;
            }

            void CodeJoin() {
                NetworkLobby.Instance.CodeJoin(root.Q<TextField>("join-lobby-code").value);
            }
        }

        private Button MultiplayerButton(string id) {
            var button = document.rootVisualElement.Q<Button>(id);
            multiplayerButtons.Add(button);
            return button;
        }

        private void SetMultiplayerButtonsEnabled(string playerName) {
            var empty = string.IsNullOrEmpty(playerName);
            foreach (var button in multiplayerButtons) {
                button.SetEnabled(!empty);
            }
        }
    }
}
