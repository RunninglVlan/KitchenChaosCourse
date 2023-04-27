using KitchenChaos.Services;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class GameOver : UIService {
        [SerializeField, Scene] private string loadingScene = null!;

        private Label orderCount = null!;

        void Start() {
            orderCount = document.rootVisualElement.Q<Label>("order-count");
            Hide();
            GameService.Instance.StateChanged += SetVisibleOnGameOver;
            document.rootVisualElement.Q<Button>("reload").clicked += Reload;
        }

        private void SetVisibleOnGameOver() {
            SetVisible(GameService.Instance.IsGameOver);
            if (GameService.Instance.IsGameOver) {
                orderCount.text = DeliveryService.Instance.DeliveredOrders.ToString();
            }
        }

        private void Reload() {
            var gameScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadSceneAsync(loadingScene).completed += _ => {
                SceneManager.LoadScene(gameScene);
            };
        }
    }
}
