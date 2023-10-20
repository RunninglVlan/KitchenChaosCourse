using System.Collections.Generic;
using KitchenChaos.Services;
using Unity.Services.Lobbies.Models;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public partial class Lobbies : UIService {
        private VisualElement lobbyContainer = null!;

        void Start() {
            Hide();
            lobbyContainer = document.rootVisualElement.Q<VisualElement>("lobbies");
            NetworkLobby.Instance.LobbiesChanged += ListLobbies;
            document.rootVisualElement.Q<Button>("close").clicked += Hide;
        }

        private void ListLobbies(List<Lobby> lobbies) {
            lobbyContainer.Clear();
            foreach (var lobby in lobbies) {
                lobbyContainer.Add(CreateLobbyButton(lobby));
            }
            return;

            Button CreateLobbyButton(Lobby lobby) {
                var button = new Button(() => NetworkLobby.Instance.IdJoin(lobby.Id));
                button.AddToClassList("button");
                button.text = lobby.Name;
                return button;
            }
        }

        void OnDestroy() {
            NetworkLobby.Instance.LobbiesChanged -= ListLobbies;
        }
    }
}
