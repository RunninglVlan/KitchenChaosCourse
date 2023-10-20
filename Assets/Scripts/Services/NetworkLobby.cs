using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace KitchenChaos.Services {
    public class NetworkLobby : MonoSingleton<NetworkLobby> {
        private const float HEARTBEAT = 15;
        private const float QUERY_LOBBIES = 3;

        public event Action<List<Lobby>> LobbiesChanged = delegate { };
        public event Action StartedCreating = delegate { };
        public event Action<string> FailedCreating = delegate { };
        public event Action<string> FailedToJoin = delegate { };

        private float heartbeatTimer = HEARTBEAT;
        private float queryLobbiesTimer = QUERY_LOBBIES;
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

        private async void InitUnityAuth() {
            if (UnityServices.State == ServicesInitializationState.Initialized) {
                return;
            }
            var options = new InitializationOptions();
            options.SetProfile(Guid.NewGuid().ToString()[..30]);
            await UnityServices.InitializeAsync(options);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

        void Update() {
            if (Joined != null) {
                SendHeartbeat();
            } else {
                QueryLobbies();
            }
        }

        private void SendHeartbeat() {
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

        private void QueryLobbies() {
            if (!AuthenticationService.Instance.IsSignedIn) {
                return;
            }
            queryLobbiesTimer -= Time.deltaTime;
            if (queryLobbiesTimer < 0) {
                queryLobbiesTimer = QUERY_LOBBIES;
                QueryLobbiesAsync();
            }

            async void QueryLobbiesAsync() {
                var availableLobbies = new QueryLobbiesOptions {
                    Filters = new List<QueryFilter> {
                        new(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
                    }
                };
                try {
                    var response = await LobbyService.Instance.QueryLobbiesAsync(availableLobbies);
                    LobbiesChanged(response.Results);
                } catch (LobbyServiceException e) {
                    Debug.Log(e);
                }
            }
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

        public async void IdJoin(string value) {
            try {
                Joined = await LobbyService.Instance.JoinLobbyByIdAsync(value);
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

        private async void Leave() {
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
            } catch (LobbyServiceException e) {
                Debug.Log(e);
            }
        }

        void OnDestroy() => LeaveLobby();

        public void LeaveLobby() {
            if (IsHost()) {
                Delete();
            } else {
                Leave();
            }
        }
    }
}
