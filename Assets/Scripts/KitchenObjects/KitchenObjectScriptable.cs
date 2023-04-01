using UnityEngine;

namespace KitchenObjects {
    [CreateAssetMenu(fileName = nameof(KitchenObjectScriptable),
        menuName = "Scriptable/" + nameof(KitchenObjectScriptable))]
    public class KitchenObjectScriptable : ScriptableObject {
        public KitchenObject prefab = null!;
        public Sprite sprite = null!;
    }
}
