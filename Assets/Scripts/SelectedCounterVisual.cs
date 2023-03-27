using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {
    [SerializeField] private Counter counter = null!;
    [SerializeField] private GameObject visual = null!;

    void Start() {
        Player.Instance.SelectedCounterChanged += SetVisualActive;
    }

    private void SetVisualActive(Counter? selectedCounter) {
        visual.SetActive(selectedCounter == counter);
    }
}
