using System;
using KitchenObjects;
using UnityEngine;

namespace Counters {
    public class CuttingCounter : Counter {
        [SerializeField] private CuttingRecipe[] recipes = Array.Empty<CuttingRecipe>();
        [SerializeField] private ProgressBar progressBar = null!;
        [SerializeField] private CuttingCounterVisual visual = null!;

        private int cuts;

        public override void Interact(Player player) {
            var counterHasObject = TryGetKitchenObject(out var counterObject);
            if (player.TryGetKitchenObject(out var playerObject)) {
                if (counterHasObject) {
                    TryAddToPlate(playerObject, counterObject);
                    return;
                }
                if (!TryGetRecipe(playerObject.Scriptable, out _)) {
                    return;
                }
                playerObject.Parent = this;
                cuts = 0;
                progressBar.Set(0);
            } else if (counterHasObject && progressBar.IsEmptyOrFilled) {
                counterObject.Parent = player;
                progressBar.Set(0);
            }
        }

        public override void InteractAlternate() {
            if (!TryGetKitchenObject(out var counterObject)) {
                return;
            }
            if (!TryGetRecipe(counterObject.Scriptable, out var recipe)) {
                return;
            }
            cuts++;
            progressBar.Set((float)cuts / recipe.maxCuts);
            visual.Cut();
            if (cuts < recipe.maxCuts) {
                return;
            }
            counterObject.DestroySelf();
            KitchenObject.Spawn(recipe.output, this);
        }

        private bool TryGetRecipe(KitchenObjectScriptable input, out CuttingRecipe result) {
            result = null!;
            foreach (var recipe in recipes) {
                if (recipe.input != input) {
                    continue;
                }
                result = recipe;
                return true;
            }
            return false;
        }
    }
}
