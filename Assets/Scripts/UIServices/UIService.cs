using UnityEngine;
using UnityEngine.UIElements;

namespace KitchenChaos.UIServices {
    public abstract class UIService : MonoBehaviour {
        [SerializeField] protected UIDocument document = null!;

        protected virtual void Awake() {
            document.gameObject.SetActive(true);
        }

        public void Show() => SetVisible(true);
        protected void Hide() => SetVisible(false);
        protected void SetVisible(bool value) => document.rootVisualElement.SetActive(value);
    }
}
