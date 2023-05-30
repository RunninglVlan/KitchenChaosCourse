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
            root.Q<Button>("create-lobby").clicked += CreateLobby;
            root.Q<Button>("join-lobby").clicked += JoinLobby;

            void Create() {
                NetworkService.Instance.StartHost();
                SceneService.Instance.LoadCharacterSelection();
            }

            void CreateLobby() {
                NetworkLobby.Instance.Create("LobbyName", false);
            }

            void JoinLobby() {
                NetworkLobby.Instance.QuickJoin();
            }
        }
    }
}
