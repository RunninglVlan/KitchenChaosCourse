using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Services {
    public class Options : UIService {
        private const string CONTROL_VECTOR = "Vector2";
        private const string ESC = "<Keyboard>/escape";

        [SerializeField] private VisualTreeAsset controlAsset = null!;

        private VisualElement rebindingOverlay = null!;

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

            void Hide() => root.SetActive(false);
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
                    AddAction(action, action.name);
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

        private void AddAction(InputAction action, string actionName, int binding = 0) {
            var element = controlAsset.Instantiate();
            document.rootVisualElement.Q<VisualElement>("controls").Add(element);
            element.Q<Label>().text = actionName;
            var button = element.Q<Button>();
            SetBindingText();
            button.clicked += () => {
                Rebind(action, binding, SetBindingText);
            };

            void SetBindingText() {
                button.text = action.GetBindingDisplayString(binding);
            }
        }

        private void Rebind(InputAction action, int binding, Action completeAction) {
            rebindingOverlay.SetActive(true);
            action.Disable();
            action.PerformInteractiveRebinding(binding)
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

        public void Show() {
            document.rootVisualElement.SetActive(true);
        }
    }
}
