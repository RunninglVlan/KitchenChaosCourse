using UnityEngine;

namespace Counters {
    public class StoveCounterSound : MonoBehaviour {
        [SerializeField] private StoveCounter counter = null!;

        private AudioSource audioSource = null!;

        void Awake() {
            audioSource = GetComponent<AudioSource>();
            counter.ActiveChanged += SetEffectsActive;
        }

        private void SetEffectsActive(bool value) {
            if (value) {
                audioSource.Play();
            } else {
                audioSource.Pause();
            }
        }
    }
}
