using Unity.Netcode;
using UnityEngine.UIElements;

namespace KitchenChaos.Services {
    public class Testing : UIService {
        void Start() {
            var root = document.rootVisualElement;
            root.Q<Button>("host").clicked += StartHost;
            root.Q<Button>("client").clicked += StartClient;

            void StartHost() {
                NetworkManager.Singleton.StartHost();
                root.SetActive(false);
            }

            void StartClient() {
                NetworkManager.Singleton.StartClient();
                root.SetActive(false);
            }
        }
    }
}
