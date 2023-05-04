using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos {
    public class DestroyNetwork : MonoBehaviour {
        void Awake() {
            var network = NetworkManager.Singleton;
            if (!network) {
                return;
            }
            NetworkManager.Singleton.Shutdown();
            Destroy(NetworkManager.Singleton.gameObject);
        }
    }
}
