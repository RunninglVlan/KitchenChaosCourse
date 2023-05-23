using KitchenChaos.KitchenObjects;
using KitchenChaos.Players;
using KitchenChaos.Services;

namespace KitchenChaos.Counters {
    public partial class DeliveryCounter : Counter {
        public override void Interact(Player player) {
            if (!player.TryGetKitchenObject(out var playerObject)) {
                return;
            }
            if (playerObject is not PlateObject plate) {
                return;
            }
            DeliveryService.Instance.Deliver(plate);
            KitchenObjectService.Instance.Destroy(playerObject);
        }
    }
}
