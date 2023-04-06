using KitchenChaos.Services;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Counters {
    public class DeliveryCounterVisual : MonoBehaviour {
        private const float MAX_SHOWING_SECONDS = 1;

        [SerializeField] private Image image = null!;
        [SerializeField] private Sprite success = null!;
        [SerializeField] private Sprite fail = null!;
        [SerializeField] private Color successColor = Color.green;
        [SerializeField] private Color failColor = Color.red;

        private float showing;

        void Start() {
            gameObject.SetActive(false);
            DeliveryService.Instance.DeliverySucceeded += ShowSuccess;
            DeliveryService.Instance.DeliveryFailed += ShowFail;

            void ShowSuccess() => Show(success, successColor);
            void ShowFail() => Show(fail, failColor);

            void Show(Sprite sprite, Color color) {
                image.sprite = sprite;
                image.color = color;
                gameObject.SetActive(true);
                showing = MAX_SHOWING_SECONDS;
            }
        }

        void Update() {
            if (showing > 0) {
                showing -= Time.deltaTime;
                return;
            }
            gameObject.SetActive(false);
        }
    }
}
