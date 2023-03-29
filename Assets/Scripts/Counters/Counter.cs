using System;
using KitchenObjects;
using UnityEngine;

namespace Counters {
    public abstract class Counter : KitchenObjectParent {
        public static event Action<Counter> ObjectPlaced = delegate { };

        [SerializeField] private Transform top = null!;

        public override Transform ObjectLocation => top;

        public abstract void Interact(Player player);
        public virtual void InteractAlternate() { }

        protected static bool TryAddToPlate(KitchenObject mainObject, KitchenObject ingredient) {
            if (mainObject is not PlateObject plate) {
                return false;
            }
            var added = plate.TryAddIngredient(ingredient.Scriptable);
            if (added) {
                ingredient.DestroySelf();
            }
            return added;
        }

        public override void SetKitchenObject(KitchenObject value) {
            base.SetKitchenObject(value);
            ObjectPlaced(this);
        }
    }
}
