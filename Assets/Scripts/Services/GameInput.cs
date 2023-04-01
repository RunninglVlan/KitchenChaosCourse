using UnityEngine;
using UnityEngine.InputSystem;

namespace Services {
    public class GameInput : MonoBehaviour {
        private const string BINDING_OVERRIDES = "BindingOverrides";

        public InputActions Actions { get; private set; } = null!;

        private static string BindOverrides {
            get => PlayerPrefs.GetString(BINDING_OVERRIDES);
            set => PlayerPrefs.SetString(BINDING_OVERRIDES, value);
        }

        public static GameInput Instance { get; private set; } = null!;

        void Awake() {
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
            Actions = new InputActions();
            Actions.LoadBindingOverridesFromJson(BindOverrides);
        }

        public void Save() => BindOverrides = Actions.SaveBindingOverridesAsJson();

        void OnEnable() => Actions.Enable();
        void OnDisable() => Actions.Dispose();
    }
}
