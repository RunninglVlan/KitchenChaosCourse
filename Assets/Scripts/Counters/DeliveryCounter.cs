using KitchenObjects;
using UnityEngine;

namespace Counters {
    public class DeliveryCounter : Counter {
        public static DeliveryCounter Instance { get; private set; } = null!;

        void Awake() {
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
        }

        public override void Interact(Player player) {
            if (!player.TryGetKitchenObject(out var playerObject)) {
                return;
            }
            if (playerObject is not PlateObject plate) {
                return;
            }
            DeliveryManager.Instance.Deliver(plate);
            playerObject.DestroySelf();
        }
    }
}
