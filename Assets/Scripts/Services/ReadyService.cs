using System.Collections.Generic;
using Unity.Netcode;

namespace KitchenChaos.Services {
    public class ReadyService : NetworkSingleton<ReadyService> {
        private readonly List<ulong> playerReadyStates = new();

        public void SetPlayerReady() {
            SetPlayerReadyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams parameters = default) {
            playerReadyStates.Add(parameters.Receive.SenderClientId);
            foreach (var client in NetworkManager.Singleton.ConnectedClientsIds) {
                if (!playerReadyStates.Contains(client)) {
                    return;
                }
            }
            SceneService.Instance.LoadGame(network: true);
        }
    }
}
