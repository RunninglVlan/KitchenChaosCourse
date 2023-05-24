using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos {
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
        public static T Instance { get; private set; } = null!;

        protected virtual void Awake() {
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = (T)this;
        }
    }

    public abstract class NetworkSingleton<T> : NetworkBehaviour where T : NetworkSingleton<T> {
        public static T Instance { get; protected set; } = null!;

        protected virtual void Awake() {
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = (T)this;
        }
    }
}
