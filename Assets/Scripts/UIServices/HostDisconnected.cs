using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class HostDisconnected : UIService {
        private Label reason = null!;

        void Start() {
            Hide();
            var root = document.rootVisualElement;
            reason = root.Q<Label>("reason");
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            root.Q<Button>("menu").clicked += SceneService.Instance.LoadMainMenu;
        }

        private void OnClientDisconnected(ulong client) {
            var show = client == NetworkManager.ServerClientId;
            SetVisible(show);
            reason.SetActive(false);
            if (show && !string.IsNullOrEmpty(NetworkManager.Singleton.DisconnectReason)) {
                reason.SetActive(true);
                reason.text = NetworkManager.Singleton.DisconnectReason;
            }
        }

        void OnDestroy() {
            var network = NetworkManager.Singleton;
            if (!network) {
                return;
            }
            network.OnClientDisconnectCallback -= OnClientDisconnected;
        }
    }
}
