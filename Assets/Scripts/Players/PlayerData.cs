using System;
using Unity.Collections;
using Unity.Netcode;

namespace KitchenChaos.Players {
    public struct PlayerData : IEquatable<PlayerData>, INetworkSerializable {
        public ulong clientId;
        public int colorIndex;
        public FixedString64Bytes name;
        public FixedString64Bytes playerId;

        public bool Equals(PlayerData other) {
            return clientId == other.clientId &&
                   colorIndex == other.colorIndex && name == other.name && playerId == other.playerId;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref clientId);
            serializer.SerializeValue(ref colorIndex);
            serializer.SerializeValue(ref name);
            serializer.SerializeValue(ref playerId);
        }
    }
}
