using System;
using UnityEngine;

namespace Counters {
    public class StoveCounterVisual : MonoBehaviour {
        [SerializeField] private GameObject[] effects = Array.Empty<GameObject>();

        public void SetEffectsActive(bool value) {
            foreach (var effect in effects) {
                effect.SetActive(value);
            }
        }
    }
}
