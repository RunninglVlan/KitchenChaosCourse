using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Services {
    public class ColorService : NetworkSingleton<ColorService> {
        private readonly Color[] colors = { Color.red, Color.green, Color.blue, Color.yellow };
        private readonly Dictionary<ulong, Color> clientColors = new();

        public void RequestColor() {
            RequestColorServerRpc(new ServerRpcParams());
        }

        [ServerRpc(RequireOwnership = false)]
        private void RequestColorServerRpc(ServerRpcParams parameters) {
            var clientId = parameters.Receive.SenderClientId;
            if (!clientColors.TryGetValue(clientId, out var color)) {
                color = colors.GetRandom();
                while (clientColors.ContainsValue(color)) {
                    color = colors.GetRandom();
                }
                clientColors[clientId] = color;
            }
            var networkColors = clientColors.Select(it => new NetworkColor(it.Key, it.Value)).ToArray();
            SetColorClientRpc(networkColors);
        }

        [ClientRpc]
        [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local", Justification = "Rpc can't be static")]
        private void SetColorClientRpc(NetworkColor[] networkColors) {
            foreach (var player in FindObjectsByType<PlayerVisual>(FindObjectsSortMode.None)) {
                foreach (var clientColor in networkColors) {
                    player.SetColor(clientColor.client, clientColor.color);
                }
            }
        }

        private struct NetworkColor : INetworkSerializeByMemcpy {
            public readonly ulong client;
            public readonly Color color;

            public NetworkColor(ulong client, Color color) {
                this.client = client;
                this.color = color;
            }
        }
    }
}
