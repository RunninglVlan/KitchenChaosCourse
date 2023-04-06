using UnityEngine;

namespace KitchenChaos {
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T> {
        public static T Instance { get; private set; } = null!;

        protected virtual void Awake() {
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = (T)this;
        }
    }
}
