using System;
using System.Collections.Generic;
using Unity.Netcode;

namespace KitchenChaos.Services {
    public abstract class BaseReadyService<T> : NetworkSingleton<T> where T : NetworkSingleton<T> {
        public abstract event Action PlayerBecameReadyOnServer;

        protected readonly List<ulong> playerReadyStates = new();
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
        public sealed override event Action PlayerBecameReadyOnServer = delegate { };
        public event Action PlayerBecameReadyOnClient = delegate { };

        protected override Action ReadyAction => () => {
            NetworkLobby.Instance.Delete();
            SceneService.Instance.LoadGame();
        };

        public void SetPlayerReady() {
            PlayerBecameReadyOnServer();
            SetPlayerReadyServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams parameters = default) {
            SetPlayerReadyClientRpc(parameters.Receive.SenderClientId);
            BaseSetPlayerReady(parameters.Receive.SenderClientId);
        }

        [ClientRpc]
        private void SetPlayerReadyClientRpc(ulong client) {
            playerReadyStates.Add(client);
            PlayerBecameReadyOnClient();
        }

        public bool IsPlayerReady(ulong client) {
            return playerReadyStates.Contains(client);
        }
    }
}
