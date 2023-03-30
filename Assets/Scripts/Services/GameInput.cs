using UnityEngine;

namespace Services {
    public class GameInput : MonoBehaviour {
        public InputActions Actions { get; private set; } = null!;

        public static GameInput Instance { get; private set; } = null!;

        void Awake() {
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
            Actions = new InputActions();
            Actions.Enable();
        }
    }
}
