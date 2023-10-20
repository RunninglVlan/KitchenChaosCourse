using KitchenChaos.Services;
using UnityEngine;

namespace KitchenChaos {
    public class DestroyLobby : MonoBehaviour {
        void Awake() {
            var lobby = NetworkLobby.Instance;
            if (!lobby) {
                return;
            }
            Destroy(NetworkLobby.Instance.gameObject);
        }
    }
}
