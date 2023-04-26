using KitchenChaos.Services;
using UnityEngine;

namespace KitchenChaos.Counters {
    public class StoveCounterSounds : MonoBehaviour {
        private const float MAX_WARNING_SECONDS = .2f;

        [SerializeField] private StoveCounter counter = null!;

        private AudioSource audioSource = null!;
        private bool playingWarning;
        private float warningSeconds;

        void Awake() {
            audioSource = GetComponent<AudioSource>();
            counter.StateChanged += SetEffectsActive;
            counter.WarningSet += StartWarning;

            void StartWarning() => playingWarning = true;
        }

        private void SetEffectsActive(StoveCounter.State state) {
            if (state is not StoveCounter.State.Idle) {
                audioSource.Play();
            } else {
                audioSource.Pause();
            }
            if (state is StoveCounter.State.Idle or StoveCounter.State.Burned) {
                playingWarning = false;
            }
        }

        void Update() {
            if (!playingWarning) {
                return;
            }
            warningSeconds += Time.deltaTime;
            if (warningSeconds < MAX_WARNING_SECONDS) {
                return;
            }
            warningSeconds = 0;
            SoundService.Instance.PlayWarning(counter.transform.position);
        }
    }
}
