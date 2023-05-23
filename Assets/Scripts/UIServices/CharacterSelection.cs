using KitchenChaos.Services;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class CharacterSelection : UIService {
        void Start() {
            var root = document.rootVisualElement;
            root.Q<Button>("main-menu").clicked += GoToMainMenu;
            var ready = root.Q<Button>("ready");
            ready.Focus();
            ready.clicked += SetReady;

            void GoToMainMenu() {
                SceneService.Instance.LoadMainMenu();
            }

            void SetReady() {
                Hide();
                ReadyService.Instance.SetPlayerReady();
            }
        }
    }
}
