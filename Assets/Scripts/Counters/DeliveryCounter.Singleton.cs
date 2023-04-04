using UnityEngine;

namespace Counters {
    public partial class DeliveryCounter {
        public static DeliveryCounter Instance { get; private set; } = null!;

        void Awake() {
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
        }
    }
}
