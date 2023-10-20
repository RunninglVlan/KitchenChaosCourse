using UnityEngine;

namespace KitchenChaos.UIServices {
    public partial class Lobbies {
        public static Lobbies Instance { get; private set; } = null!;

        protected override void Awake() {
            base.Awake();
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
        }
    }
}
