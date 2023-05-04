using System.Collections.Generic;
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

        public void LoadGame(bool network = false) => Load(game, network);
        public void LoadMainMenu() => Load(mainMenu, false);
        public void LoadCharacterSelection(bool network = false) => Load(characterSelection, network);

        private void Load(string scene, bool network) {
            if (!network) {
                SceneManager.LoadSceneAsync(loading).completed += _ => {
                    SceneManager.LoadScene(scene);
                };
                return;
            }
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnCompleted;
            NetworkManager.Singleton.SceneManager.LoadScene(loading, LoadSceneMode.Single);

            void OnCompleted(string _, LoadSceneMode __, List<ulong> ___, List<ulong> ____) {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= OnCompleted;
                NetworkManager.Singleton.SceneManager.LoadScene(scene, LoadSceneMode.Single);
            }
        }
    }
}
