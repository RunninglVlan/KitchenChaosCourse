using System.Collections.Generic;
using System.Linq;
using KitchenChaos.Players;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Services {
    public class PlayerInitService : NetworkSingleton<PlayerInitService> {
        [SerializeField] Vector2[] positions = { new(-2, 1), new(2, 1), new(-2, -2), new(2, -2) };

        private readonly Dictionary<ulong, int> clientPositions = new();

        public void RequestPosition() => RequestPositionServerRpc();

        [ServerRpc(RequireOwnership = false)]
        private void RequestPositionServerRpc(ServerRpcParams parameters = default) {
            var client = parameters.Receive.SenderClientId;
            if (!clientPositions.TryGetValue(client, out var randomPosition)) {
                randomPosition = RandomPosition();
                while (ClientPositionContains(randomPosition)) {
                    randomPosition = RandomPosition();
                }
                clientPositions[client] = randomPosition;
            }
            var networkData = clientPositions.Select(it => new NetworkPosition(it.Key, it.Value)).ToArray();
            SetPositionClientRpc(networkData);

            int RandomPosition() => Random.Range(0, positions.Length);

            bool ClientPositionContains(int foundPosition) {
                foreach (var position in clientPositions.Values) {
                    if (foundPosition == position) {
                        return true;
                    }
                }
                return false;
            }
        }

        [ClientRpc]
        private void SetPositionClientRpc(NetworkPosition[] data) {
            foreach (var player in FindObjectsByType<PlayerInit>(FindObjectsSortMode.None)) {
                foreach (var networkData in data) {
                    player.Init(networkData.client, positions[networkData.position]);
                }
            }
        }

        private struct NetworkPosition : INetworkSerializeByMemcpy {
            public readonly ulong client;
            public readonly int position;

            public NetworkPosition(ulong client, int position) {
                this.client = client;
                this.position = position;
            }
        }
    }
}
