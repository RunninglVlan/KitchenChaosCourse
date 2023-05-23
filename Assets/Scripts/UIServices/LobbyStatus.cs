using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class LobbyStatus : UIService {
        private Label message = null!;

        void Start() {
            Hide();
            NetworkService.Instance.TryingToJoin += ShowConnecting;
            NetworkService.Instance.FailedToJoin += ShowDisconnected;
            message = document.rootVisualElement.Q<Label>("message");
            document.rootVisualElement.Q<Button>("close").clicked += Hide;
        }

        private void ShowConnecting() {
            Show();
            message.text = "Connecting...";
            message.EnableInClassList("cs-error", false);
        }

        private void ShowDisconnected() {
            Show();
            message.text = NetworkManager.Singleton.DisconnectReason;
            if (string.IsNullOrEmpty(message.text)) {
                message.text = "Disconnected";
            }
            message.EnableInClassList("cs-error", true);
        }

        void OnDestroy() {
            NetworkService.Instance.TryingToJoin -= ShowConnecting;
            NetworkService.Instance.FailedToJoin -= ShowDisconnected;
        }
    }
}
