using System;
using NaughtyAttributes;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KitchenChaos.Services {
    public class SceneService : MonoSingleton<SceneService> {
        [SerializeField, Scene] private string mainMenu = null!;
        [SerializeField, Scene] private string loading = null!;
        [SerializeField, Scene] private string game = null!;
        [SerializeField, Scene] private string lobby = null!;
        [SerializeField, Scene] private string characterSelection = null!;

        public void LoadGame() {
            SceneManager.LoadSceneAsync(loading).completed += _ => {
                SceneManager.LoadScene(game);
            };
        }

        public void LoadMainMenu() => SceneManager.LoadScene(mainMenu);

        public void LoadCharacterSelection(bool network = false) {
            if (!network) {
                throw new NotImplementedException();
            }
            NetworkManager.Singleton.SceneManager.LoadScene(characterSelection, LoadSceneMode.Single);
        }
    }
}
