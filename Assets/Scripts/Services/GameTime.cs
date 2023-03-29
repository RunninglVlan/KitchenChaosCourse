using UnityEngine.UIElements;

namespace Services {
    public class GameTime : UIService {
        private Label time = null!;

        void Start() {
            time = document.rootVisualElement.Q<Label>("time");
            SetTime((int) GameService.MAX_PLAYING_SECONDS);
        }

        void Update() {
            if (!GameService.Instance.IsPlaying) {
                return;
            }
            SetTime(GameService.Instance.PlayingSeconds);
        }

        private void SetTime(int seconds) {
            var minutes = seconds / 60;
            time.text = $"{minutes:00}:{seconds % 60:00}";
        }
    }
}
