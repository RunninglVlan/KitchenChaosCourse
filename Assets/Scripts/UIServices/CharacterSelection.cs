using KitchenChaos.Services;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace KitchenChaos.UIServices {
    public class CharacterSelection : UIService {
        void Start() {
            var root = document.rootVisualElement;
            root.Q<Button>("main-menu").clicked += GoToMainMenu;
            ShowLobby();
            ShowRelayCode();
            var ready = root.Q<Button>("ready");
            ready.Focus();
            ready.clicked += SetReady;
            var colorButtons = root.Query<Button>(className: "color").ToList();
            for (var i = 0; i < colorButtons.Count; i++) {
                ColorElement(colorButtons[i], i);
            }
            return;

            void GoToMainMenu() {
                NetworkLobby.Instance.LeaveLobby();
                SceneService.Instance.LoadMainMenu();
            }

            void SetReady() {
                Hide();
                ReadyService.Instance.SetPlayerReady();
            }
        }

        private void ShowLobby() {
            var root = document.rootVisualElement;
            var lobby = NetworkLobby.Instance.Joined;
            root.Q<VisualElement>("lobby").SetActive(lobby != null);
            if (lobby == null) {
                return;
            }
            root.Q<TextField>("lobby-name").value = lobby.Name;
            root.Q<TextField>("lobby-code").value = lobby.LobbyCode;
        }

        private void ShowRelayCode() {
            var root = document.rootVisualElement;
            var relay = root.Q<VisualElement>("relay");
            var show = NetworkLobby.Instance.Joined == null && LobbyUI.RelayCode != null;
            relay.SetActive(show);
            if (!show) {
                return;
            }
            root.Q<TextField>("relay-code").value = LobbyUI.RelayCode;
        }

        private static void ColorElement(Button button, int index) {
            button.style.backgroundColor = NetworkService.Instance.PlayerColor(index);
            SelectColor();
            button.clicked += ChangeColor;
            NetworkService.Instance.OnPlayerDataChanged += SelectColor;

            void ChangeColor() {
                NetworkService.Instance.ChangePlayerColor(index);
            }

            void SelectColor() {
                var selected = NetworkService.Instance.PlayerData().colorIndex == index;
                button.EnableInClassList("cs-selected", selected);
            }
        }
    }
}
