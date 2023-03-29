using UnityEngine.UIElements;

public class GameOver : UIService {
    private Label orderCount = null!;

    void Start() {
        orderCount = document.rootVisualElement.Q<Label>("order-count");
        document.rootVisualElement.SetActive(false);
        GameService.Instance.StateChanged += SetActive;
    }

    private void SetActive() {
        document.rootVisualElement.SetActive(GameService.Instance.IsGameOver);
        if (GameService.Instance.IsGameOver) {
            orderCount.text = DeliveryService.Instance.DeliveredOrders.ToString();
        }
    }
}
