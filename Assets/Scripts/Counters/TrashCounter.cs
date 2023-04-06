using KitchenChaos.Services;

namespace KitchenChaos.Counters {
    public class TrashCounter : Counter {
        public override void Interact(Player player) {
            if (!player.TryGetKitchenObject(out var playerObject)) {
                return;
            }
            playerObject.DestroySelf();
            SoundService.Instance.PlayTrash(this);
        }
    }
}
