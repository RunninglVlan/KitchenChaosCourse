using System;
using JetBrains.Annotations;
using UnityEngine;

namespace KitchenObjects {
    public class PlateCompleteVisual : MonoBehaviour {
        [SerializeField] private IngredientObject[] ingredientObjects = Array.Empty<IngredientObject>();

        public void ShowIngredient(KitchenObjectScriptable value) {
            if (!TryGetObject(value, out var ingredient)) {
                return;
            }
            ingredient.SetActive(true);
        }

        private bool TryGetObject(KitchenObjectScriptable input, out GameObject result) {
            result = null!;
            foreach (var ingredient in ingredientObjects) {
                if (ingredient.scriptable != input) {
                    continue;
                }
                result = ingredient.gameObject;
                return true;
            }
            return false;
        }

        [Serializable]
        private struct IngredientObject {
            [UsedImplicitly] public string name;
            public KitchenObjectScriptable scriptable;
            public GameObject gameObject;
        }
    }
}
