using UnityEngine;

namespace Counters {
    public class StoveCounterSounds : MonoBehaviour {
        [SerializeField] private StoveCounter counter = null!;

        private AudioSource audioSource = null!;

        void Awake() {
            audioSource = GetComponent<AudioSource>();
            counter.StateChanged += SetEffectsActive;
        }

        private void SetEffectsActive(StoveCounter.State state) {
            if (state is not StoveCounter.State.Idle) {
                audioSource.Play();
            } else {
                audioSource.Pause();
            }
        }
    }
}
