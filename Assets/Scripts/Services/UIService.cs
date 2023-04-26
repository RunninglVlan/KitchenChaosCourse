using UnityEngine;
using UnityEngine.UIElements;

namespace KitchenChaos.Services {
    public abstract class UIService : MonoBehaviour {
        [SerializeField] protected UIDocument document = null!;

        protected virtual void Awake() {
            document.gameObject.SetActive(true);
        }

        protected void Show() => SetVisible(true);
        protected void Hide() => SetVisible(false);
        protected void SetVisible(bool value) => document.rootVisualElement.SetActive(value);
    }
}
