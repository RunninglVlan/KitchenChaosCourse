using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenObjects {
    public class PlateObject : KitchenObject {
        public event Action<KitchenObjectScriptable> IngredientAdded = delegate { };

        [SerializeField] private KitchenObjectScriptable[] validIngredients = Array.Empty<KitchenObjectScriptable>();

        public List<KitchenObjectScriptable> Ingredients { get; } = new();

        public bool TryAddIngredient(KitchenObjectScriptable value) {
            if (Ingredients.Contains(value) || !validIngredients.Contains(value)) {
                return false;
            }
            Ingredients.Add(value);
            IngredientAdded(value);
            return true;
        }
    }
}
