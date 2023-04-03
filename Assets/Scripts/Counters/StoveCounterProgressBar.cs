using UnityEngine;

namespace Counters {
    public class StoveCounterProgressBar : ProgressBar {
        private const float MAX_FLASHING_SECONDS = .1f;

        [SerializeField] private StoveCounter counter = null!;
        [SerializeField] private Color normalColor = Color.yellow;
        [SerializeField] private Color warningColor = Color.red;

        private bool flashing, warning;
        private float flashingSeconds;

        void Awake() {
            counter.WarningSet += StartFlashing;
            counter.StateChanged += StopFlashing;

            void StartFlashing() => flashing = true;

            void StopFlashing(StoveCounter.State state) {
                if (state == StoveCounter.State.Burned) {
                    flashing = false;
                }
            }
        }

        void Update() {
            if (!flashing) {
                return;
            }
            flashingSeconds += Time.deltaTime;
            if (flashingSeconds < MAX_FLASHING_SECONDS) {
                return;
            }
            flashingSeconds = 0;
            warning = !warning;
            progress.color = warning ? warningColor : normalColor;
        }
    }
}
