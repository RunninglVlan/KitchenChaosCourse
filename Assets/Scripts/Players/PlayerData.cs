using System;
using Unity.Netcode;

namespace KitchenChaos.Players {
    public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable {
        public ulong clientId;
        public int colorIndex;

        public bool Equals(PlayerData other) {
            return clientId == other.clientId && colorIndex == other.colorIndex;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref colorIndex);
        }
    }
}
