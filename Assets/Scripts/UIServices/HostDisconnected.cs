using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class HostDisconnected : UIService {
        void Start() {
            Hide();
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            document.rootVisualElement.Q<Button>("menu").clicked += SceneService.Instance.LoadMainMenu;
        }

        private void OnClientDisconnected(ulong client) {
            SetVisible(client == NetworkManager.ServerClientId);
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
