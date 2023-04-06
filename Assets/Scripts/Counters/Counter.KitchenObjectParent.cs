using KitchenChaos.KitchenObjects;
using KitchenChaos.Services;
using UnityEngine;

namespace KitchenChaos.Counters {
    public abstract partial class Counter : IKitchenObjectParent {
        [SerializeField] private Transform top = null!;

        private KitchenObject? kitchenObject;
        public Transform ObjectLocation => top;

        public bool TryGetKitchenObject(out KitchenObject foundObject) {
            foundObject = null!;
            if (!kitchenObject) {
                return false;
            }
            foundObject = kitchenObject!;
            return true;
        }

        public void ClearKitchenObject() => kitchenObject = null;

        public void SetKitchenObject(KitchenObject value) {
            kitchenObject = value;
            SoundService.Instance.PlayDrop(this);
        }

        public bool HasKitchenObject() => kitchenObject;
    }
}
