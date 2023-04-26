using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Services {
    public class PlayerInitService : NetworkSingleton<PlayerInitService> {
        [SerializeField] Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow };
        [SerializeField] Vector2[] positions = { new(-2, 1), new(2, 1), new(-2, -2), new(2, -2) };

        private readonly Dictionary<ulong, (int color, int position)> clientData = new();

        public void RequestData() => RequestDataServerRpc();

        [ServerRpc(RequireOwnership = false)]
        private void RequestDataServerRpc(ServerRpcParams parameters = default) {
            var clientId = parameters.Receive.SenderClientId;
            if (!clientData.TryGetValue(clientId, out var data)) {
                data = RandomData();
                while (ClientDataContains(data)) {
                    data = RandomData();
                }
                clientData[clientId] = data;
            }
            var networkData = clientData.Select(it => new NetworkData(it.Key, it.Value)).ToArray();
            SetDataClientRpc(networkData);

            (int, int) RandomData() => (Random.Range(0, colors.Length), Random.Range(0, positions.Length));

            bool ClientDataContains((int color, int position) foundData) {
                foreach (var (color, position) in clientData.Values) {
                    if (foundData.color == color || foundData.position == position) {
                        return true;
                    }
                }
                return false;
            }
        }

        [ClientRpc]
        private void SetDataClientRpc(NetworkData[] data) {
            foreach (var player in FindObjectsByType<PlayerInit>(FindObjectsSortMode.None)) {
                foreach (var networkData in data) {
                    player.Init(networkData.client, colors[networkData.color], positions[networkData.position]);
                }
            }
        }

        private struct NetworkData : INetworkSerializeByMemcpy {
            public readonly ulong client;
            public readonly int color;
            public readonly int position;

            public NetworkData(ulong client, (int color, int position) data) {
                this.client = client;
                color = data.color;
                position = data.position;
            }
        }
    }
}
