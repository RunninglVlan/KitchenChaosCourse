using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Services {
    public class Options : UIService {
        private const string CONTROL_VECTOR = "Vector2";
        private const string ESC = "<Keyboard>/escape";
        private const string KEYBOARD = "<Keyboard>";
        private const string GAMEPAD = "<Gamepad>";

        [SerializeField] private VisualTreeAsset controlAsset = null!;

        private VisualElement rebindingOverlay = null!;
        private Action hideAction = null!;

        public static Options Instance { get; private set; } = null!;

        protected override void Awake() {
            base.Awake();
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
        }

        void Start() {
            var root = document.rootVisualElement;
            AddOptionSlider(root.Q<Slider>("sounds"), SoundService.Instance.Volume, SoundVolume);
            AddOptionSlider(root.Q<Slider>("music"), Music.Instance.Volume, MusicVolume);
            AddControls();
            rebindingOverlay = root.Q<VisualElement>("overlay");
            rebindingOverlay.SetActive(false);
            root.Q<Button>("hide").clicked += Hide;
            GameService.Instance.Unpaused += Hide;
            root.SetActive(false);

            void SoundVolume(float value) {
                SoundService.Instance.Volume = value;
            }

            void MusicVolume(float value) {
                Music.Instance.Volume = value;
            }

            void Hide() {
                hideAction();
                root.SetActive(false);
            }
        }

        private static void AddOptionSlider(Slider root, float value, Action<float> callback) {
            root.RegisterValueChangedCallback(evt => {
                var newValue = evt.newValue / 10;
                callback(newValue);
                root.Q<Label>("value").text = newValue.ToString("F1");
            });
            root.value = value * 10;
        }

        private void AddControls() {
            var map = GameInput.Instance.Actions.asset.actionMaps.First();
            foreach (var action in map.actions) {
                if (action.expectedControlType == CONTROL_VECTOR) {
                    AddCompositeActions(action);
                } else {
                    var keyboardBinding = 0;
                    int? gamepadBinding = null;
                    for (var index = 0; index < action.bindings.Count; index++) {
                        var binding = action.bindings[index];
                        if (!binding.path.Contains(GAMEPAD)) {
                            keyboardBinding = index;
                        } else {
                            gamepadBinding = index;
                        }
                    }
                    AddAction(action, action.name, keyboardBinding, gamepadBinding);
                }
            }

            void AddCompositeActions(InputAction action) {
                for (var index = 1; index < action.bindings.Count; index++) {
                    var binding = action.bindings[index];
                    if (!binding.isPartOfComposite) {
                        break;
                    }
                    AddAction(action, $"{action.name} {binding.name.ToCamel()}", index);
                }
            }
        }

        private void AddAction(InputAction action, string actionName, int keyboardBinding, int? gamepadBinding = null) {
            var element = controlAsset.Instantiate();
            document.rootVisualElement.Q<VisualElement>("controls").Add(element);
            element.Q<Label>().text = actionName;
            var keyboardButton = element.Q<Button>("keyboard");
            SetBindingText(keyboardButton, keyboardBinding);
            keyboardButton.clicked += () => {
                Rebind(action, keyboardBinding, KEYBOARD, () => SetBindingText(keyboardButton, keyboardBinding));
            };
            var gamepadButton = element.Q<Button>("gamepad");
            gamepadButton.visible = gamepadBinding.HasValue;
            if (!gamepadBinding.HasValue) {
                return;
            }
            SetBindingText(gamepadButton, gamepadBinding.Value);
            gamepadButton.clicked += () => {
                Rebind(action, gamepadBinding.Value, GAMEPAD,
                    () => SetBindingText(gamepadButton, gamepadBinding.Value));
            };

            void SetBindingText(Button button, int binding) {
                button.text = action.GetBindingDisplayString(binding);
            }
        }

        private void Rebind(InputAction action, int binding, string devicePath, Action completeAction) {
            rebindingOverlay.SetActive(true);
            var deviceInput = devicePath == KEYBOARD ? "Keyboard key" : "Controller button";
            rebindingOverlay.Q<Label>("overlay-label").text = $"Waiting for the {deviceInput}";
            action.Disable();
            action.PerformInteractiveRebinding(binding)
                .WithControlsHavingToMatchPath(devicePath)
                .WithCancelingThrough(ESC)
                .OnCancel(OnEnd)
                .OnComplete(operation => {
                    OnEnd(operation);
                    completeAction();
                    GameInput.Instance.Save();
                })
                .Start();

            void OnEnd(InputActionRebindingExtensions.RebindingOperation operation) {
                operation.Dispose();
                action.Enable();
                rebindingOverlay.SetActive(false);
            }
        }

        [SuppressMessage("ReSharper", "ParameterHidesMember")]
        public void Show(Action hideAction) {
            this.hideAction = hideAction;
            document.rootVisualElement.SetActive(true);
        }
    }
}
