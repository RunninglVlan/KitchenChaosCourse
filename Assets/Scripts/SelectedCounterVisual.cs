using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {
    [SerializeField] private ClearCounter counter = null!;
    [SerializeField] private GameObject visual = null!;

    void Start() {
        Player.Instance.SelectedCounterChanged += SetVisualActive;
    }

    private void SetVisualActive(ClearCounter? selectedCounter) {
        visual.SetActive(selectedCounter == counter);
    }
}
