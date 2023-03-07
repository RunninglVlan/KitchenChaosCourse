using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {
    [SerializeField] private ClearCounter counter;
    [SerializeField] private GameObject visual;

    void Start() {
        Player.Instance.SelectedCounterChanged += SetVisualActive;
    }

    private void SetVisualActive(ClearCounter selectedCounter) {
        visual.SetActive(selectedCounter == counter);
    }
}
