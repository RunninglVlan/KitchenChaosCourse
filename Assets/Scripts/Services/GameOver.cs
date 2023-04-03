using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Services {
    public class GameOver : UIService {
        [SerializeField, Scene] private string loadingScene = null!;

        private Label orderCount = null!;

        void Start() {
            orderCount = document.rootVisualElement.Q<Label>("order-count");
            document.rootVisualElement.SetActive(false);
            GameService.Instance.StateChanged += SetActive;
            document.rootVisualElement.Q<Button>("reload").clicked += Reload;
        }

        private void SetActive() {
            document.rootVisualElement.SetActive(GameService.Instance.IsGameOver);
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
