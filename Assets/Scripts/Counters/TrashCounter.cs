using System;

namespace Counters {
    public class TrashCounter : Counter {
        public static event Action<TrashCounter> Trashed = delegate { };

        public override void Interact(Player player) {
            if (!player.TryGetKitchenObject(out var playerObject)) {
                return;
            }
            playerObject.DestroySelf();
            Trashed(this);
        }
    }
}
