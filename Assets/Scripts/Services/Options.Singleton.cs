using UnityEngine;

namespace Services {
    public partial class Options {
        public static Options Instance { get; private set; } = null!;

        protected override void Awake() {
            base.Awake();
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
        }
    }
}
