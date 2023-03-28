using UnityEngine;

namespace KitchenObjects {
    public abstract class KitchenObjectParent : MonoBehaviour {
        private KitchenObject? kitchenObject;
        public abstract Transform ObjectLocation { get; }

        public bool TryGetKitchenObject(out KitchenObject foundObject) {
            foundObject = null!;
            if (!kitchenObject) {
                return false;
            }
            foundObject = kitchenObject!;
            return true;
        }

        public void ClearKitchenObject() => kitchenObject = null;
        public void SetKitchenObject(KitchenObject value) => kitchenObject = value;
        public bool HasKitchenObject() => kitchenObject;
    }
}
