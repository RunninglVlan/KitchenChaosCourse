using System.Linq;
using KitchenChaos.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class Tutorial : UIService {
        private const string BUTTON_CLASS = "cs-button";

        [SerializeField] private VisualTreeAsset controlAsset = null!;

        private VisualElement controls = null!;

        void Start() {
            controls = document.rootVisualElement.Q<VisualElement>("controls");
            Options.Instance.ControlsRebound += AddControls;
            GameService.Instance.PlayerBecameReady += Hide;
            AddControls();
        }

        private void AddControls() {
            for (var index = controls.childCount - 1; index >= 0; index--) {
                controls.RemoveAt(index);
            }
            var map = GameInput.Instance.Actions.asset.actionMaps.First();
            foreach (var action in map.actions) {
                if (action.expectedControlType == Options.CONTROL_VECTOR) {
                    AddCompositeAction(action);
                } else {
                    AddAction(action);
                }
            }
        }

        private void AddCompositeAction(InputAction action) {
            var element = controlAsset.Instantiate();
            controls.Add(element);
            var index = 1;
            for (; index < action.bindings.Count; index++) {
                var binding = action.bindings[index];
                if (!binding.isPartOfComposite) {
                    break;
                }
                AddKey(element, action, index);
            }
            for (; index < action.bindings.Count; index++) {
                var binding = action.bindings[index];
                if (!binding.path.Contains(Options.GAMEPAD)) {
                    continue;
                }
                AddButton(element, action, index);
            }
        }

        private void AddAction(InputAction action) {
            var element = controlAsset.Instantiate();
            controls.Add(element);
            for (var index = 0; index < action.bindings.Count; index++) {
                var binding = action.bindings[index];
                if (binding.path.Contains(Options.KEYBOARD)) {
                    AddKey(element, action, index);
                } else {
                    AddButton(element, action, index);
                }
            }
        }

        private static void AddKey(VisualElement root, InputAction action, int binding) {
            AddButton(root, action, binding, "keyboard");
        }

        private static void AddButton(VisualElement root, InputAction action, int binding, string control = "gamepad") {
            var button = new Label(action.GetBindingDisplayString(binding));
            button.AddToClassList(BUTTON_CLASS);
            root.Q<VisualElement>(control).Add(button);
        }
    }
}
