using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class HostDisconnected : UIService {
        void Start() {
            Hide();
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            document.rootVisualElement.Q<Button>("reload").clicked += Reload;

            void OnClientDisconnected(ulong client) {
                SetVisible(client == NetworkManager.ServerClientId);
            }

            void Reload() {
                NetworkManager.Singleton.Shutdown();
                Destroy(NetworkManager.Singleton.gameObject);
                SceneService.Instance.LoadGame();
            }
        }
    }
}
