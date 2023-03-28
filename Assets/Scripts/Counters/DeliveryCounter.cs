using KitchenObjects;

namespace Counters {
    public class DeliveryCounter : Counter {
        public override void Interact(Player player) {
            if (!player.TryGetKitchenObject(out var playerObject)) {
                return;
            }
            if (playerObject is not PlateObject) {
                return;
            }
            playerObject.DestroySelf();
        }
    }
}
