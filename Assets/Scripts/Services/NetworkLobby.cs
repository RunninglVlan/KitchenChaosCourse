﻿using System;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace KitchenChaos.Services {
    public class NetworkLobby : MonoSingleton<NetworkLobby> {
        private const float HEARTBEAT = 15;

        public event Action StartedCreating = delegate { };
        public event Action<string> FailedCreating = delegate { };
        public event Action<string> FailedToJoin = delegate { };

        private float heartbeatTimer = HEARTBEAT;
        public Lobby? Joined { get; private set; }

        protected override void Awake() {
            if (Instance) {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            base.Awake();
            InitUnityAuth();
        }

        void Update() {
            if (!IsHost()) {
                return;
            }
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer < 0) {
                heartbeatTimer = HEARTBEAT;
                LobbyService.Instance.SendHeartbeatPingAsync(Joined!.Id);
            }
        }

        private bool IsHost() {
            return Joined != null && Joined.HostId == AuthenticationService.Instance.PlayerId;
        }

        private async void InitUnityAuth() {
            if (UnityServices.State == ServicesInitializationState.Initialized) {
                return;
            }
            var options = new InitializationOptions();
            options.SetProfile(Guid.NewGuid().ToString()[..30]);
            await UnityServices.InitializeAsync(options);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        public async void Create(string lobbyName, bool isPrivate) {
            try {
                StartedCreating();
                var options = new CreateLobbyOptions { IsPrivate = isPrivate };
                Joined = await LobbyService.Instance.CreateLobbyAsync(lobbyName, NetworkService.MAX_PLAYERS, options);
                NetworkService.Instance.StartHost();
                SceneService.Instance.LoadCharacterSelection();
            } catch (LobbyServiceException e) {
                FailedCreating(e.Message.ToCamel());
                Debug.Log(e);
            }
        }

        public async void QuickJoin() {
            try {
                Joined = await LobbyService.Instance.QuickJoinLobbyAsync();
                NetworkService.Instance.StartClient();
            } catch (LobbyServiceException e) {
                Debug.Log(e);
                FailedToJoin(e.Message.ToCamel());
            }
        }

        public async void CodeJoin(string value) {
            try {
                Joined = await LobbyService.Instance.JoinLobbyByCodeAsync(value);
                NetworkService.Instance.StartClient();
            } catch (LobbyServiceException e) {
                Debug.Log(e);
                FailedToJoin(e.Message.ToCamel());
            }
        }

        public async void Delete() {
            if (Joined == null) {
                return;
            }
            try {
                await LobbyService.Instance.DeleteLobbyAsync(Joined.Id);
                Joined = null;
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }

        public async void Leave() {
            if (Joined == null) {
                return;
            }
            try {
                await LobbyService.Instance.RemovePlayerAsync(Joined.Id, AuthenticationService.Instance.PlayerId);
                Joined = null;
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }

        public async void KickPlayer(string playerId) {
            if (Joined == null) {
                return;
            }
            try {
                await LobbyService.Instance.RemovePlayerAsync(Joined.Id, playerId);
                Joined = null;
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }

        void OnDestroy() {
            if (!IsHost()) {
                return;
            }
            Delete();
        }
    }
}
