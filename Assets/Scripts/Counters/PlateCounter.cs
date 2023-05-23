using KitchenChaos.KitchenObjects;
using KitchenChaos.Players;
using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Counters {
    public class PlateCounter : Counter {
        private const float MAX_SPAWN_SECONDS = 4;
        private const float MAX_SPAWNED_PLATES = 4;

        [SerializeField] private KitchenObjectScriptable plate = null!;
        [SerializeField] private PlateCounterVisual visual = null!;

        private float spawnSeconds;
        private int spawnedPlates;

        public override void Interact(Player player) {
            if (spawnedPlates <= 0 || player.HasKitchenObject()) {
                return;
            }
            KitchenObjectService.Instance.Spawn(plate, player);
            DestroyTopServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyTopServerRpc() {
            DestroyTopClientRpc();
        }

        [ClientRpc]
        private void DestroyTopClientRpc() {
            visual.DestroyTop();
            spawnedPlates--;
        }

        void Update() {
            if (!IsServer) {
                return;
            }
            if (!GameService.Instance.IsPlaying || spawnedPlates >= MAX_SPAWNED_PLATES) {
                return;
            }
            spawnSeconds += Time.deltaTime;
            if (spawnSeconds < MAX_SPAWN_SECONDS) {
                return;
            }
            spawnSeconds = 0;
            SpawnPlateClientRpc();
        }

        [ClientRpc]
        private void SpawnPlateClientRpc() {
            spawnedPlates++;
            visual.Spawn();
        }
    }
}
