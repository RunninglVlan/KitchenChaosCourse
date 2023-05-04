using System;
using Unity.Netcode;

namespace KitchenChaos.Services {
    public static class NetworkService {
        private const int MAX_PLAYERS = 4;

        public static event Action TryingToJoin = delegate { };
        public static event Action FailedToJoin = delegate { };

        public static void StartHost() {
            NetworkManager.Singleton.ConnectionApprovalCallback += ProcessConnectionApproval;
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
        }

        public static void StartClient() {
            TryingToJoin();
            NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnected;
            NetworkManager.Singleton.StartClient();

            static void OnDisconnected(ulong _) {
                FailedToJoin();
            }
        }
    }
}
