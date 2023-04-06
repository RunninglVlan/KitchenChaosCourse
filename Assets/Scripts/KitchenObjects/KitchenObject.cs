using UnityEngine;

namespace KitchenChaos.KitchenObjects {
    public class KitchenObject : MonoBehaviour {
        [SerializeField] private KitchenObjectScriptable scriptable = null!;

        public KitchenObjectScriptable Scriptable => scriptable;
        private IKitchenObjectParent? parent;

        public IKitchenObjectParent Parent {
            set {
                parent?.ClearKitchenObject();
                parent = value;
                if (parent.HasKitchenObject()) {
                    Debug.LogError($"Parent already has a {nameof(KitchenObject)}!");
                }
                parent.SetKitchenObject(this);
                var objectTransform = transform;
                objectTransform.parent = parent.ObjectLocation;
                objectTransform.localPosition = Vector3.zero;
            }
        }

        public static void Spawn(KitchenObjectScriptable scriptable, IKitchenObjectParent parent) {
            var instance = Instantiate(scriptable.prefab);
            instance.Parent = parent;
        }

        public void DestroySelf() {
            parent!.ClearKitchenObject();
            Destroy(gameObject);
        }
    }
}
