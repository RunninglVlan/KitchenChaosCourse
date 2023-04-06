using System;
using UnityEngine;

namespace KitchenChaos.Counters {
    public class StoveCounterVisual : MonoBehaviour {
        [SerializeField] private StoveCounter counter = null!;
        [SerializeField] private GameObject[] effects = Array.Empty<GameObject>();

        void Awake() {
            counter.StateChanged += SetEffectsActive;
        }

        private void SetEffectsActive(StoveCounter.State state) {
            foreach (var effect in effects) {
                effect.SetActive(state is not StoveCounter.State.Idle);
            }
        }
    }
}
