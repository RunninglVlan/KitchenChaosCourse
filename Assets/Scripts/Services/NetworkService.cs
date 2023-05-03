using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Services {
    public class NetworkService : MonoBehaviour {
        public static void StartHost() {
            NetworkManager.Singleton.ConnectionApprovalCallback += ProcessConnectionApproval;
            NetworkManager.Singleton.StartHost();

            void ProcessConnectionApproval(NetworkManager.ConnectionApprovalRequest _,
                NetworkManager.ConnectionApprovalResponse response
            ) {
                response.Approved = GameService.Instance.IsWaitingToStart;
                response.CreatePlayerObject = true;
            }
        }

        public static void StartClient() => NetworkManager.Singleton.StartClient();
    }
}
