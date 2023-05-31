using KitchenChaos.Services;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace KitchenChaos.UIServices {
    public class CharacterSelection : UIService {
        void Start() {
            var root = document.rootVisualElement;
            root.Q<Button>("main-menu").clicked += SceneService.Instance.LoadMainMenu;
            ShowLobby();
            var ready = root.Q<Button>("ready");
            ready.Focus();
            ready.clicked += SetReady;
            var colorButtons = root.Query<Button>(className: "color").ToList();
            for (var i = 0; i < colorButtons.Count; i++) {
                ColorElement(colorButtons[i], i);
            }

            void ShowLobby() {
                var lobby = NetworkLobby.Instance.Joined;
                root.Q<VisualElement>("lobby").SetActive(lobby != null);
                if (lobby == null) {
                    return;
                }
                root.Q<TextField>("lobby-name").value = lobby.Name;
                root.Q<TextField>("lobby-code").value = lobby.LobbyCode;
            }

            void SetReady() {
                Hide();
                ReadyService.Instance.SetPlayerReady();
            }
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
