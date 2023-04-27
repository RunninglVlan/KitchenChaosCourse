using KitchenChaos.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class GameCountdown : UIService {
        private const string SMALL = "cs-small";

        private Label counter = null!;
        private int previousSeconds;
        private bool secondsChanged;

        protected override void Awake() {
            base.Awake();
            Hide();
            GameService.Instance.StateChanged += SetVisibleOnCountdown;

            void SetVisibleOnCountdown() {
                SetVisible(GameService.Instance.IsCountingDownToStart);
            }
        }

        void Start() {
            counter = document.rootVisualElement.Q<Label>("counter");
        }

        void Update() {
            if (!GameService.Instance.IsCountingDownToStart) {
                return;
            }
            var seconds = GameService.Instance.CountdownSeconds;
            counter.text = seconds.ToString();
            if (secondsChanged) {
                counter.AddToClassList(SMALL);
                secondsChanged = false;
            }
            if (previousSeconds == seconds) {
                return;
            }
            SoundService.Instance.PlayWarning(Vector3.zero);
            counter.RemoveFromClassList(SMALL);
            previousSeconds = seconds;
            secondsChanged = true;
        }
    }
}
