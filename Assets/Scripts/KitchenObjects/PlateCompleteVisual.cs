using System;
using JetBrains.Annotations;
using UnityEngine;

namespace KitchenChaos.KitchenObjects {
    public class PlateCompleteVisual : MonoBehaviour {
        [SerializeField] private PlateObject plateObject = null!;
        [SerializeField] private IngredientObject[] ingredientObjects = Array.Empty<IngredientObject>();

        void Awake() {
            plateObject.IngredientAdded += ShowIngredient;
        }

        private void ShowIngredient(KitchenObjectScriptable value) {
            foreach (var ingredient in ingredientObjects) {
                if (ingredient.scriptable != value) {
                    continue;
                }
                ingredient.gameObject.SetActive(true);
            }
        }

        [Serializable]
        private struct IngredientObject {
            [UsedImplicitly] public string name;
            public KitchenObjectScriptable scriptable;
            public GameObject gameObject;
        }
    }
}
