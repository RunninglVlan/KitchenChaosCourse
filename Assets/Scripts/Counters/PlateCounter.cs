using KitchenObjects;
using UnityEngine;

namespace Counters {
    public class PlateCounter : Counter {
        private const float MAX_SPAWN_TIME = 4;
        private const float MAX_SPAWNED_PLATES = 4;

        [SerializeField] private KitchenObjectScriptable plate = null!;
        [SerializeField] private PlateCounterVisual visual = null!;

        private float spawnTime;
        private int spawnedPlates;

        public override void Interact(Player player) {
            if (spawnedPlates <= 0 || player.HasKitchenObject()) {
                return;
            }
            KitchenObject.Spawn(plate, player);
            visual.DestroyTop();
            spawnedPlates--;
        }

        void Update() {
            if (spawnedPlates >= MAX_SPAWNED_PLATES) {
                return;
            }
            spawnTime += Time.deltaTime;
            if (spawnTime < MAX_SPAWN_TIME) {
                return;
            }
            spawnTime = 0;
            spawnedPlates++;
            visual.Spawn();
        }
    }
}
