using KitchenChaos.KitchenObjects;
using UnityEngine;

namespace KitchenChaos.Counters {
    public abstract partial class Counter : MonoBehaviour {
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
    }
}
