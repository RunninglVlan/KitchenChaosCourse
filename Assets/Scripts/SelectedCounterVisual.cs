using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {
    [SerializeField] private Counter counter = null!;
    [SerializeField] private GameObject[] visuals = null!;

    void Start() {
        Player.Instance.SelectedCounterChanged += SetVisualActive;
    }

    private void SetVisualActive(Counter? selectedCounter) {
        foreach (var visual in visuals) {
            visual.SetActive(selectedCounter == counter);
        }
    }
}
