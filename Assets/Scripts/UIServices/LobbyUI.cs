﻿using System.Collections.Generic;
using KitchenChaos.Services;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class LobbyUI : UIService {
        private const string IP = "IP:PORT";
        private const string CODE = "CODE";

        public static string PlayerName {
            get => PlayerPrefs.GetString("PlayerName");
            private set => PlayerPrefs.SetString("PlayerName", value);
        }

        public static bool UseRelay { get; private set; }
        public static string? RelayCode { get; set; }

        private readonly List<Button> multiplayerButtons = new();
        private TextField joinCode = null!;

        void Start() {
            var root = document.rootVisualElement;
            root.Q<Button>("main-menu").clicked += SceneService.Instance.LoadMainMenu;
            var playerName = root.Q<TextField>("player-name");
            playerName.RegisterValueChangedCallback(OnNameChanged);
            var useRelay = root.Q<Toggle>("use-relay");
            UseRelay = useRelay.value;
            RelayCode = null;
            useRelay.RegisterValueChangedCallback(OnUseRelayChanged);
            var create = MultiplayerButton("create");
            create.Focus();
            create.clicked += Create;
            joinCode = root.Q<TextField>("join-code");
            joinCode.textEdition.placeholder = IP;
            MultiplayerButton("join").clicked += Join;
            MultiplayerButton("create-lobby").clicked += CreateLobby.Instance.Show;
            MultiplayerButton("lobbies").clicked += Lobbies.Instance.Show;
            MultiplayerButton("quick-join").clicked += NetworkLobby.Instance.QuickJoin;
            MultiplayerButton("code-join").clicked += CodeJoin;
            SetMultiplayerButtonsEnabled(playerName.value = PlayerName);
        }

        private static async void Create() {
            Allocation? relayAllocation = null;
            if (UseRelay) {
                relayAllocation = await Relay.Allocate();
                var joinCode = await Relay.JoinCode(relayAllocation);
                RelayCode = joinCode;
            }
            NetworkService.Instance.StartHost(relayAllocation);
            SceneService.Instance.LoadCharacterSelection();
        }

        private async void Join() {
            JoinAllocation? relayAllocation = null;
            if (UseRelay) {
                relayAllocation = await Relay.Join(joinCode.value);
                RelayCode = joinCode.value;
            }
            NetworkService.Instance.StartClient(relayAllocation, joinCode.value);
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

        private void OnUseRelayChanged(ChangeEvent<bool> evt) {
            joinCode.textEdition.placeholder = evt.newValue ? CODE : IP;
            UseRelay = evt.newValue;
        }

        private void OnNameChanged(ChangeEvent<string> evt) {
            SetMultiplayerButtonsEnabled(evt.newValue);
            PlayerName = evt.newValue;
        }

        private void CodeJoin() {
            var code = document.rootVisualElement.Q<TextField>("join-lobby-code");
            NetworkLobby.Instance.CodeJoin(code.value);
        }
    }
}
