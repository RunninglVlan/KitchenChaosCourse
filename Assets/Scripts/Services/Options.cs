using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Services {
    public class Options : UIService {
        public static Options Instance { get; private set; } = null!;

        void Awake() {
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
        }

        void Start() {
            var root = document.rootVisualElement;
            AddOptionSlider(root.Q<Slider>("sounds"), SoundService.Instance.Volume, SoundVolume);
            AddOptionSlider(root.Q<Slider>("music"), Music.Instance.Volume, MusicVolume);
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

        public void Show() {
            document.rootVisualElement.SetActive(true);
        }
    }
}
