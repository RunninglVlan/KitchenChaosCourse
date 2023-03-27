using UnityEngine;

namespace Counters {
    public abstract class Counter : KitchenObjectParent {
        [SerializeField] private Transform top = null!;

        public override Transform ObjectLocation => top;

        public abstract void Interact(Player player);
        public virtual void InteractAlternate() { }
    }
}
