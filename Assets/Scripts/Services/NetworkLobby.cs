using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KitchenChaos.UIServices;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using UnityEngine;
using static Unity.Services.Lobbies.Models.DataObject;

namespace KitchenChaos.Services {
    public class NetworkLobby : MonoSingleton<NetworkLobby> {
        private const float HEARTBEAT = 15;
        private const float QUERY_LOBBIES = 3;
        private const string KEY_CODE = "JOIN_CODE";

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
            return;

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

        public async void Create(string lobbyName, bool isPrivate, bool local) {
            try {
                StartedCreating();
                var options = new CreateLobbyOptions { IsPrivate = isPrivate };
                Joined = await LobbyService.Instance.CreateLobbyAsync(lobbyName, NetworkService.MAX_PLAYERS, options);
                Allocation? relayAllocation = null;
                if (LobbyUI.UseRelay) {
                    relayAllocation = await Relay.Allocate();
                }
                NetworkService.Instance.StartHost(relayAllocation, local);
                var joinCode = await JoinCode(relayAllocation);
                await LobbyService.Instance.UpdateLobbyAsync(Joined.Id, new UpdateLobbyOptions {
                    Data = new Dictionary<string, DataObject> {
                        { KEY_CODE, new DataObject(VisibilityOptions.Member, joinCode) }
                    }
                });
                SceneService.Instance.LoadCharacterSelection();
            } catch (LobbyServiceException e) {
                FailedCreating(e.Message.ToCamel());
                Debug.Log(e);
            }
            return;

            async Task<string> JoinCode(Allocation? relayAllocation) {
                if (relayAllocation == null) {
                    var connectionData = NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData;
                    return $"{connectionData.Address}:{connectionData.Port}";
                }
                var code = await Relay.JoinCode(relayAllocation);
                LobbyUI.RelayCode = code;
                return code;
            }
        }

        public async void QuickJoin() {
            try {
                Joined = await LobbyService.Instance.QuickJoinLobbyAsync();
                await StartClient(Joined);
            } catch (LobbyServiceException e) {
                Debug.Log(e);
                FailedToJoin(e.Message.ToCamel());
            }
        }

        public async void CodeJoin(string value) {
            try {
                Joined = await LobbyService.Instance.JoinLobbyByCodeAsync(value);
                await StartClient(Joined);
            } catch (LobbyServiceException e) {
                Debug.Log(e);
                FailedToJoin(e.Message.ToCamel());
            }
        }

        public async void IdJoin(string value) {
            try {
                Joined = await LobbyService.Instance.JoinLobbyByIdAsync(value);
                await StartClient(Joined);
            } catch (LobbyServiceException e) {
                Debug.Log(e);
                FailedToJoin(e.Message.ToCamel());
            }
        }

        private static async Task StartClient(Lobby lobby) {
            JoinAllocation? relayAllocation = null;
            var code = lobby.Data[KEY_CODE].Value;
            if (LobbyUI.UseRelay) {
                relayAllocation = await Relay.Join(code);
                LobbyUI.RelayCode = code;
            }
            NetworkService.Instance.StartClient(relayAllocation, code);
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
