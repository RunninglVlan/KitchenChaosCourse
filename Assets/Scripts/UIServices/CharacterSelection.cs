using KitchenChaos.Services;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class CharacterSelection : UIService {
        void Start() {
            var ready = document.rootVisualElement.Q<Button>("ready");
            ready.Focus();
            ready.clicked += SetReady;

            void SetReady() {
                Hide();
                ReadyService.Instance.SetPlayerReady();
            }
        }
    }
}
