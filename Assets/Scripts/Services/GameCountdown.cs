using UnityEngine;
using UnityEngine.UIElements;

namespace KitchenChaos.Services {
    public class GameCountdown : UIService {
        private const string SMALL = "cs-small";

        private Label counter = null!;
        private int previousSeconds;
        private bool secondsChanged;

        protected override void Awake() {
            base.Awake();
            document.rootVisualElement.SetActive(false);
            GameService.Instance.StateChanged += SetActive;

            void SetActive() {
                document.rootVisualElement.SetActive(GameService.Instance.IsCountingDownToStart);
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
