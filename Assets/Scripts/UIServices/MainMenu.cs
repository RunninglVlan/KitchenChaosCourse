using KitchenChaos.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class MainMenu : UIService {
        void Start() {
            var play = document.rootVisualElement.Q<Button>("play");
            play.Focus();
            play.clicked += SceneService.Instance.LoadGame;
            document.rootVisualElement.Q<Button>("quit").clicked += Application.Quit;
        }
    }
}
