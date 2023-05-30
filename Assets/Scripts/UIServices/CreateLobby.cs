using KitchenChaos.Services;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public partial class CreateLobby : UIService {
        void Start() {
            Hide();
            var root = document.rootVisualElement;
            var lobbyName = root.Q<TextField>("lobby-name");
            lobbyName.Focus();
            root.Q<Button>("create-private").clicked += CreatePrivate;
            root.Q<Button>("create-public").clicked += CreatePublic;
            root.Q<Button>("close").clicked += Hide;

            void CreatePrivate() {
                NetworkLobby.Instance.Create(lobbyName.value, true);
            }

            void CreatePublic() {
                NetworkLobby.Instance.Create(lobbyName.value, false);
            }
        }
    }
}
