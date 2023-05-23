using KitchenChaos.Players;
using KitchenChaos.Services;
using Unity.Netcode;

namespace KitchenChaos.Counters {
    public class TrashCounter : Counter {
        public override void Interact(Player player) {
            if (!player.TryGetKitchenObject(out var playerObject)) {
                return;
            }
            KitchenObjectService.Instance.Destroy(playerObject);
            PlayInteractServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlayInteractServerRpc() {
            PlayInteractClientRpc();
        }

        [ClientRpc]
        private void PlayInteractClientRpc() {
            SoundService.Instance.PlayTrash(this);
        }
    }
}
