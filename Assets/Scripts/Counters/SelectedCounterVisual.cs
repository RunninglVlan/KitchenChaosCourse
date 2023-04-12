using UnityEngine;

namespace KitchenChaos.Counters {
    public class SelectedCounterVisual : MonoBehaviour {
        [SerializeField] private Counter counter = null!;
        [SerializeField] private GameObject[] visuals = null!;

        void Start() {
            if (Player.LocalInstance) {
                SubscribeToSelectedCounterChanged();
            } else {
                Player.LocalInstanceSet += SubscribeToSelectedCounterChanged;
            }
        }

        private void SubscribeToSelectedCounterChanged() {
            Player.LocalInstance.SelectedCounterChanged += SetVisualActive;
        }

        void OnDestroy() {
            Player.LocalInstanceSet -= SubscribeToSelectedCounterChanged;
        }

        private void SetVisualActive(Counter? selectedCounter) {
            foreach (var visual in visuals) {
                visual.SetActive(selectedCounter == counter);
            }
        }
    }
}
