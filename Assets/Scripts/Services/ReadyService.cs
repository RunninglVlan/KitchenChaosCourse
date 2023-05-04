using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace KitchenChaos.Services {
    public abstract class BaseReadyService<T> : NetworkSingleton<T> where T : NetworkSingleton<T> {
        private readonly List<ulong> playerReadyStates = new();
        protected abstract Action ReadyAction { get; }

        protected void BaseSetPlayerReady(ulong senderClient) {
            playerReadyStates.Add(senderClient);
            foreach (var client in NetworkManager.Singleton.ConnectedClientsIds) {
                if (!playerReadyStates.Contains(client)) {
                    return;
                }
            }
            ReadyAction();
        }
    }

    public class ReadyService : BaseReadyService<ReadyService> {
        protected override Action ReadyAction => () => SceneService.Instance.LoadGame();

        public void SetPlayerReady() {
            SetPlayerReadyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams parameters = default) {
            BaseSetPlayerReady(parameters.Receive.SenderClientId);
        }
    }
}
