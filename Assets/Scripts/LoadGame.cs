using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos {
    public class LoadGame : MonoBehaviour {
        void Awake() {
            NetworkManager.Singleton.StartHost();
            SceneService.Instance.LoadGame();
        }
    }
}
