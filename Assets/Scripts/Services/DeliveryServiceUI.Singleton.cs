using UnityEngine;

namespace KitchenChaos.Services {
    public partial class DeliveryServiceUI {
        public static DeliveryServiceUI Instance { get; private set; } = null!;

        protected override void Awake() {
            base.Awake();
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
        }
    }
}
