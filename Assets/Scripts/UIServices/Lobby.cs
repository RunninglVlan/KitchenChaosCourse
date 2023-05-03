using KitchenChaos.Services;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class Lobby : UIService {
        void Start() {
            var create = document.rootVisualElement.Q<Button>("create");
            create.Focus();
            create.clicked += Create;
            document.rootVisualElement.Q<Button>("join").clicked += NetworkService.StartClient;

            void Create() {
                NetworkService.StartHost();
                SceneService.Instance.LoadCharacterSelection(network: true);
            }
        }
    }
}
