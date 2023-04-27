using KitchenChaos.Services;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class GameOver : UIService {
        private Label orderCount = null!;

        void Start() {
            orderCount = document.rootVisualElement.Q<Label>("order-count");
            Hide();
            GameService.Instance.StateChanged += SetVisibleOnGameOver;
            document.rootVisualElement.Q<Button>("reload").clicked += SceneService.Instance.LoadGame;
        }

        private void SetVisibleOnGameOver() {
            SetVisible(GameService.Instance.IsGameOver);
            if (GameService.Instance.IsGameOver) {
                orderCount.text = DeliveryService.Instance.DeliveredOrders.ToString();
            }
        }
    }
}
