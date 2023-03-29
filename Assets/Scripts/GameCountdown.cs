using UnityEngine.UIElements;

public class GameCountdown : UIService {
    private Label counter = null!;

    void Start() {
        counter = document.rootVisualElement.Q<Label>("counter");
        document.rootVisualElement.SetActive(false);
        GameService.Instance.StateChanged += SetActive;
    }

    private void SetActive() {
        document.rootVisualElement.SetActive(GameService.Instance.IsCountingDownToStart);
    }

    void Update() {
        if (!GameService.Instance.IsCountingDownToStart) {
            return;
        }
        counter.text = GameService.Instance.CountdownSeconds.ToString();
    }
}
