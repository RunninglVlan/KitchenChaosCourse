using System;
using KitchenChaos.Players;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Services {
    public class NetworkService : NetworkSingleton<NetworkService> {
        public const int MAX_PLAYERS = 4;

        [SerializeField] private PlayerColors playerColors = null!;

        public event Action TryingToJoin = delegate { };
        public event Action FailedToJoin = delegate { };
        public event Action OnPlayerDataChanged = delegate { };

        private NetworkList<PlayerData> playerData = null!;

        protected override void Awake() {
            if (Instance) {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            base.Awake();
            playerData = new();
            playerData.OnListChanged += TriggerChanged;

            void TriggerChanged(NetworkListEvent<PlayerData> _) {
                OnPlayerDataChanged();
            }
        }

        public void StartHost() {
            NetworkManager.Singleton.ConnectionApprovalCallback += ProcessConnectionApproval;
            NetworkManager.Singleton.OnClientConnectedCallback += ProcessConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += ProcessDisconnect;
            NetworkManager.Singleton.StartHost();

            void ProcessConnectionApproval(NetworkManager.ConnectionApprovalRequest _,
                NetworkManager.ConnectionApprovalResponse response
            ) {
                if (NetworkManager.Singleton.IsServer) {
                    response.Approved = true;
                    return;
                }
                if (!SceneService.Instance.IsCharacterSelection) {
                    response.Approved = false;
                    response.Reason = "Game has already started";
                    return;
                }
                if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYERS) {
                    response.Approved = false;
                    response.Reason = "Game is full";
                    return;
                }
                response.Approved = true;
            }

            void ProcessConnect(ulong clientId) {
                playerData.Add(new PlayerData {
                    clientId = clientId, colorIndex = FirstUnusedColor()
                });
            }

            void ProcessDisconnect(ulong clientId) {
                for (var i = playerData.Count - 1; i >= 0; i--) {
                    if (playerData[i].clientId == clientId) {
                        playerData.RemoveAt(i);
                    }
                }
            }
        }

        public void StartClient() {
            TryingToJoin();
            NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnected;
            NetworkManager.Singleton.StartClient();

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

        private int PlayerDataIndex(ulong clientId) {
            for (var index = 0; index < playerData.Count; index++) {
                var data = playerData[index];
                if (data.clientId != clientId) {
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
    }
}
