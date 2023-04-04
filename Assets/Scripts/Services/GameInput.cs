using UnityEngine;
using UnityEngine.InputSystem;

namespace Services {
    public class GameInput : Singleton<GameInput> {
        private const string BINDING_OVERRIDES = "BindingOverrides";

        public InputActions Actions { get; private set; } = null!;

        private static string BindOverrides {
            get => PlayerPrefs.GetString(BINDING_OVERRIDES);
            set => PlayerPrefs.SetString(BINDING_OVERRIDES, value);
        }

        protected override void Awake() {
            base.Awake();
            Actions = new InputActions();
            Actions.LoadBindingOverridesFromJson(BindOverrides);
        }

        public void Save() => BindOverrides = Actions.SaveBindingOverridesAsJson();

        void OnEnable() => Actions.Enable();
        void OnDisable() => Actions.Dispose();
    }
}
