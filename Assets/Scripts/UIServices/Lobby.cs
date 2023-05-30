using KitchenChaos.Services;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class Lobby : UIService {
        void Start() {
            var root = document.rootVisualElement;
            var create = root.Q<Button>("create");
            create.Focus();
            create.clicked += Create;
            root.Q<Button>("join").clicked += NetworkService.Instance.StartClient;

            void Create() {
                NetworkService.Instance.StartHost();
                SceneService.Instance.LoadCharacterSelection();
            }
        }
    }
}
