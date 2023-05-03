using KitchenChaos.Services;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class Testing : UIService {
        void Start() {
            var root = document.rootVisualElement;
            root.Q<Button>("host").clicked += StartHost;
            root.Q<Button>("client").clicked += StartClient;

            void StartHost() {
                NetworkService.StartHost();
                Hide();
            }

            void StartClient() {
                NetworkService.StartClient();
                Hide();
            }
        }
    }
}
