using KitchenChaos.KitchenObjects;
using KitchenChaos.Services;
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
            visual.DestroyTop();
            spawnedPlates--;
        }

        void Update() {
            if (!GameService.Instance.IsPlaying || spawnedPlates >= MAX_SPAWNED_PLATES) {
                return;
            }
            spawnSeconds += Time.deltaTime;
            if (spawnSeconds < MAX_SPAWN_SECONDS) {
                return;
            }
            spawnSeconds = 0;
            spawnedPlates++;
            visual.Spawn();
        }
    }
}
