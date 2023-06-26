using KitchenChaos.Services;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class Lobby : UIService {
        void Start() {
            var root = document.rootVisualElement;
            root.Q<Button>("main-menu").clicked += SceneService.Instance.LoadMainMenu;
            var create = root.Q<Button>("create");
            create.Focus();
            create.clicked += Create;
            root.Q<Button>("join").clicked += NetworkService.Instance.StartClient;
            root.Q<Button>("create-lobby").clicked += CreateLobby.Instance.Show;
            root.Q<Button>("quick-join").clicked += NetworkLobby.Instance.QuickJoin;
            root.Q<Button>("code-join").clicked += CodeJoin;

            void Create() {
                NetworkService.Instance.StartHost();
                SceneService.Instance.LoadCharacterSelection();
            }

            void CodeJoin() {
                NetworkLobby.Instance.CodeJoin(root.Q<TextField>("join-code").value);
            }
        }
    }
}
