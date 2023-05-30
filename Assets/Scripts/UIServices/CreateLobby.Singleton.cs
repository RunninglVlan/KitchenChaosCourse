using UnityEngine;

namespace KitchenChaos.UIServices {
    public partial class CreateLobby {
        public static CreateLobby Instance { get; private set; } = null!;

        protected override void Awake() {
            base.Awake();
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
        }
    }
}
