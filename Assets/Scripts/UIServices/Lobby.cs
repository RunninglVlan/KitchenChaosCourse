using KitchenChaos.Services;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class Lobby : UIService {
        void Start() {
            var create = document.rootVisualElement.Q<Button>("create");
            create.Focus();
            create.clicked += Create;
            document.rootVisualElement.Q<Button>("join").clicked += NetworkService.Instance.StartClient;

            void Create() {
                NetworkService.Instance.StartHost();
                SceneService.Instance.LoadCharacterSelection();
            }
        }
    }
}
