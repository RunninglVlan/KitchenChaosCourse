using KitchenObjects;
using Services;

namespace Counters {
    public partial class DeliveryCounter : Counter {
        public override void Interact(Player player) {
            if (!player.TryGetKitchenObject(out var playerObject)) {
                return;
            }
            if (playerObject is not PlateObject plate) {
                return;
            }
            DeliveryService.Instance.Deliver(plate);
            playerObject.DestroySelf();
        }
    }
}
