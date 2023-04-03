using UnityEngine;
using UnityEngine.UIElements;

namespace Services {
    public class GameCountdown : UIService {
        private const string SMALL = "cs-small";

        private Label counter = null!;
        private int previousSeconds;
        private bool secondsChanged;

        void Start() {
            counter = document.rootVisualElement.Q<Label>("counter");
            document.rootVisualElement.SetActive(false);
            GameService.Instance.StateChanged += SetActive;
        }

        private void SetActive() {
            document.rootVisualElement.SetActive(GameService.Instance.IsCountingDownToStart);
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
