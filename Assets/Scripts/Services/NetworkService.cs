using System;
using Unity.Netcode;

namespace KitchenChaos.Services {
    public class NetworkService : NetworkSingleton<NetworkService> {
        private const int MAX_PLAYERS = 4;

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
                playerData.Add(new PlayerData { clientId = clientId });
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

        public PlayerData PlayerData(int index) => playerData[index];
    }
}
