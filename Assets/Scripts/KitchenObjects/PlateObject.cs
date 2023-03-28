using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KitchenObjects {
    public class PlateObject : KitchenObject {
        [SerializeField] private KitchenObjectScriptable[] validIngredients = Array.Empty<KitchenObjectScriptable>();
        [SerializeField] private PlateCompleteVisual visual = null!;
        [SerializeField] private PlateIcons icons = null!;

        private readonly List<KitchenObjectScriptable> ingredients = new();

        public bool TryAddIngredient(KitchenObjectScriptable value) {
            if (ingredients.Contains(value) || !validIngredients.Contains(value)) {
                return false;
            }
            ingredients.Add(value);
            visual.ShowIngredient(value);
            icons.ShowIngredients(ingredients);
            return true;
        }
    }
}
