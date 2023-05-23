using System;

namespace KitchenChaos.Players {
    public partial class Player {
        public static event Action LocalInstanceSet = delegate { };

        public static Player LocalInstance { get; private set; } = null!;

        public override void OnNetworkSpawn() {
            if (IsOwner) {
                LocalInstance = this;
                LocalInstanceSet();
            }
            if (IsServer) {
                NetworkManager.OnClientDisconnectCallback += DestroyHoldingObject;
            }
        }
    }
}
