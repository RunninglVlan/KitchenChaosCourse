using System;

namespace KitchenChaos {
    public partial class Player {
        public static event Action LocalInstanceSet = delegate { };

        public static Player LocalInstance { get; private set; } = null!;

        public override void OnNetworkSpawn() {
            if (!IsOwner) {
                return;
            }
            LocalInstance = this;
            LocalInstanceSet();
        }
    }
}
