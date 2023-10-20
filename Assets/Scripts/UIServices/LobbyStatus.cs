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
            NetworkLobby.Instance.FailedToJoin += ShowLobbyJoinFailed;
            message = document.rootVisualElement.Q<Label>("message");
            document.rootVisualElement.Q<Button>("close").clicked += Hide;
        }

        private void ShowConnecting() => ShowMessage("Connecting...", false);

        private void ShowDisconnected() {
            var text = NetworkManager.Singleton.DisconnectReason;
            if (string.IsNullOrEmpty(text)) {
                text = "Disconnected";
            }
            ShowMessage(text, true);
        }

        private void ShowLobbyJoinFailed(string error) => ShowMessage(error, true);

        private void ShowMessage(string text, bool error) {
            Show();
            message.text = text;
            message.EnableInClassList("cs-error", error);
        }

        void OnDestroy() {
            NetworkService.Instance.TryingToJoin -= ShowConnecting;
            NetworkService.Instance.FailedToJoin -= ShowDisconnected;
            NetworkLobby.Instance.FailedToJoin -= ShowLobbyJoinFailed;
        }
    }
}
