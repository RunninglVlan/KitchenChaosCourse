using UnityEngine;
using UnityEngine.UIElements;

namespace KitchenChaos.Services {
    public abstract class UIService : MonoBehaviour {
        [SerializeField] protected UIDocument document = null!;

        protected virtual void Awake() {
            document.gameObject.SetActive(true);
        }
    }
}
