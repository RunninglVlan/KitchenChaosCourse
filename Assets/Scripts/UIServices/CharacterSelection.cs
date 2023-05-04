using KitchenChaos.Services;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class CharacterSelection : UIService {
        void Start() {
            var content = document.rootVisualElement.Q<VisualElement>("content");
            var ready = content.Q<Button>("ready");
            ready.Focus();
            ready.clicked += SetReady;

            void SetReady() {
                content.AddToClassList("cs-waiting");
                ReadyService.Instance.SetPlayerReady();
            }
        }
    }
}
