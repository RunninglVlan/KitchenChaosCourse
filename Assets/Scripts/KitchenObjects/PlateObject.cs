using System;
using System.Collections.Generic;
using System.Linq;
using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.KitchenObjects {
    public class PlateObject : KitchenObject {
        public event Action<KitchenObjectScriptable> IngredientAdded = delegate { };

        [SerializeField] private KitchenObjectScriptable[] validIngredients = Array.Empty<KitchenObjectScriptable>();

        public List<KitchenObjectScriptable> Ingredients { get; } = new();

        public bool TryAddIngredient(KitchenObjectScriptable value) {
            if (Ingredients.Contains(value) || !validIngredients.Contains(value)) {
                return false;
            }
            var scriptableIndex = KitchenObjectService.Instance.GetIndex(value);
            AddIngredientServerRpc(scriptableIndex);
            return true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void AddIngredientServerRpc(int scriptableIndex) {
            AddIngredientClientRpc(scriptableIndex);
        }

        [ClientRpc]
        private void AddIngredientClientRpc(int scriptableIndex) {
            var scriptable = KitchenObjectService.Instance.Get(scriptableIndex);
            Ingredients.Add(scriptable);
            IngredientAdded(scriptable);
        }
    }
}
