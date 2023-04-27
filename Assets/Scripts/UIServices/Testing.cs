using Unity.Netcode;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class Testing : UIService {
        void Start() {
            var root = document.rootVisualElement;
            root.Q<Button>("host").clicked += StartHost;
            root.Q<Button>("client").clicked += StartClient;

            void StartHost() {
                NetworkManager.Singleton.StartHost();
                Hide();
            }

            void StartClient() {
                NetworkManager.Singleton.StartClient();
                Hide();
            }
        }
    }
}
