using System;
using UnityEngine;

namespace Counters {
    public class StoveCounterVisual : MonoBehaviour {
        [SerializeField] private StoveCounter counter = null!;
        [SerializeField] private GameObject[] effects = Array.Empty<GameObject>();

        void Awake() {
            counter.ActiveChanged += SetEffectsActive;
        }

        private void SetEffectsActive(bool value) {
            foreach (var effect in effects) {
                effect.SetActive(value);
            }
        }
    }
}
