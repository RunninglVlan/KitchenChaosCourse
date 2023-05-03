using Unity.Netcode;

namespace KitchenChaos.Services {
    public static class NetworkService {
        public static void StartHost() {
            NetworkManager.Singleton.ConnectionApprovalCallback += ProcessConnectionApproval;
            NetworkManager.Singleton.StartHost();

            void ProcessConnectionApproval(NetworkManager.ConnectionApprovalRequest _,
                NetworkManager.ConnectionApprovalResponse response
            ) {
                response.Approved = true;
            }
        }

        public static void StartClient() => NetworkManager.Singleton.StartClient();
    }
}
