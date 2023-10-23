using System;
using KitchenChaos.Players;
using KitchenChaos.UIServices;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using UnityEngine;

namespace KitchenChaos.Services {
    public class NetworkService : NetworkSingleton<NetworkService> {
        public const int MAX_PLAYERS = 4;
        private const ushort PORT = 7777;
        private const string ALLOW_REMOTE_CONNECTIONS = "0.0.0.0";

        [SerializeField] private PlayerColors playerColors = null!;

        public event Action TryingToJoin = delegate { };
        public event Action FailedToJoin = delegate { };
        public event Action OnPlayerDataChanged = delegate { };

        private NetworkList<PlayerData> playerData = null!;

        protected override void Awake() {
            if (Instance) {
                Destroy(Instance.gameObject);
                Instance = null!;
            }
            DontDestroyOnLoad(gameObject);
            base.Awake();
            playerData = new();
            playerData.OnListChanged += TriggerChanged;

            void TriggerChanged(NetworkListEvent<PlayerData> _) {
                OnPlayerDataChanged();
            }
        }

        public void StartHost(bool useRelay, bool local = false) {
            NetworkManager.Singleton.ConnectionApprovalCallback += ProcessConnectionApproval;
            NetworkManager.Singleton.OnClientConnectedCallback += ProcessConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += ProcessDisconnect;
            var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            if (!useRelay) {
                var ip = local ? IPAddress.Local() : IPAddress.Public();
                unityTransport.SetConnectionData(ip, PORT, ALLOW_REMOTE_CONNECTIONS);
            }
            NetworkManager.Singleton.StartHost();

            void ProcessConnectionApproval(NetworkManager.ConnectionApprovalRequest request,
                NetworkManager.ConnectionApprovalResponse response
            ) {
                if (request.ClientNetworkId == NetworkManager.LocalClientId) {
                    response.Approved = true;
                    return;
                }
                if (!SceneService.Instance.IsCharacterSelection) {
                    response.Approved = false;
                    response.Reason = "Game has already started";
                    return;
                }
                if (NetworkManager.ConnectedClientsIds.Count >= MAX_PLAYERS) {
                    response.Approved = false;
                    response.Reason = "Game is full";
                    return;
                }
                response.Approved = true;
            }

            void ProcessConnect(ulong client) {
                playerData.Add(new PlayerData {
                    clientId = client, colorIndex = FirstUnusedColor()
                });
                if (client != NetworkManager.ServerClientId) {
                    return;
                }
                SetPlayerNameServerRpc(LobbyUI.PlayerName);
            }
        }

        private void ProcessDisconnect(ulong client) {
            for (var i = playerData.Count - 1; i >= 0; i--) {
                if (playerData[i].clientId == client) {
                    playerData.RemoveAt(i);
                }
            }
        }

        public void StartClient(bool useRelay, string code) {
            TryingToJoin();
            NetworkManager.Singleton.OnClientConnectedCallback += OnConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnected;
            var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            if (!useRelay && IPAddress.TryGetValidAddressAndPort(code, out var data)) {
                unityTransport.SetConnectionData(data.address, ushort.Parse(data.port));
            }
            NetworkManager.Singleton.StartClient();
            return;

            void OnConnected(ulong _) {
                SetPlayerNameServerRpc(LobbyUI.PlayerName);
                SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
            }

            void OnDisconnected(ulong _) {
                FailedToJoin();
            }
        }

        public bool IsPlayerConnected(int index) {
            return index < playerData.Count;
        }

        public PlayerData PlayerDataFromClientId(ulong id) {
            foreach (var data in playerData) {
                if (data.clientId != id) {
                    continue;
                }
                return data;
            }
            return default;
        }

        private int PlayerDataIndex(ulong client) {
            for (var index = 0; index < playerData.Count; index++) {
                var data = playerData[index];
                if (data.clientId != client) {
                    continue;
                }
                return index;
            }
            return -1;
        }

        public PlayerData PlayerData() {
            return PlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
        }

        public PlayerData PlayerData(int index) => playerData[index];
        public Color PlayerColor(int index) => playerColors.Get[index];

        public void ChangePlayerColor(int index) {
            ChangePlayerColorServerRpc(index);
        }

        [ServerRpc(RequireOwnership = false)]
        private void ChangePlayerColorServerRpc(int index, ServerRpcParams serverRpcParams = default) {
            if (!IsColorAvailable(index)) {
                return;
            }
            var playerIndex = PlayerDataIndex(serverRpcParams.Receive.SenderClientId);
            var data = playerData[playerIndex];
            data.colorIndex = index;
            playerData[playerIndex] = data;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default) {
            var playerIndex = PlayerDataIndex(serverRpcParams.Receive.SenderClientId);
            var data = playerData[playerIndex];
            data.name = playerName;
            playerData[playerIndex] = data;
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default) {
            var playerIndex = PlayerDataIndex(serverRpcParams.Receive.SenderClientId);
            var data = playerData[playerIndex];
            data.playerId = playerId;
            playerData[playerIndex] = data;
        }

        private bool IsColorAvailable(int colorIndex) {
            foreach (var data in playerData) {
                if (data.colorIndex == colorIndex) {
                    return false;
                }
            }
            return true;
        }

        private int FirstUnusedColor() {
            for (var i = 0; i < playerColors.Get.Count; i++) {
                if (IsColorAvailable(i)) {
                    return i;
                }
            }
            return -1;
        }

        public void KickPlayer(ulong client) {
            NetworkManager.Singleton.DisconnectClient(client);
            ProcessDisconnect(client);
        }
    }
}
