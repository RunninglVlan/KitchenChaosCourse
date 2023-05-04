using KitchenChaos.Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public class MainMenu : UIService {
        void Start() {
            var root = document.rootVisualElement;
            var game = root.Q<Button>("game");
            game.Focus();
            game.clicked += SceneService.Instance.LoadSinglePlayer;
            root.Q<Button>("lobby").clicked += SceneService.Instance.LoadLobby;
            root.Q<Button>("quit").clicked += Application.Quit;
        }
    }
}
