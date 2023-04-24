using System;
using KitchenChaos.KitchenObjects;
using KitchenChaos.Services;
using UnityEngine;

namespace KitchenChaos.Counters {
    public class CuttingCounter : Counter, IHasProgress {
        public event Action<float> ProgressSet = delegate { };

        [SerializeField] private CuttingRecipe[] recipes = Array.Empty<CuttingRecipe>();
        [SerializeField] private CuttingCounterVisual visual = null!;

        private int cuts;
        private float progress;

        public override void Interact(Player player) {
            var counterHasObject = TryGetKitchenObject(out var counterObject);
            var canTake = Mathf.Approximately(progress, 0) || Mathf.Approximately(progress, 1);
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
                ProgressSet(progress = 0);
            } else if (counterHasObject && canTake) {
                counterObject.Parent = player;
                ProgressSet(progress = 0);
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
            ProgressSet(progress = (float)cuts / recipe.maxCuts);
            visual.Cut();
            SoundService.Instance.PlayChop(this);
            if (cuts < recipe.maxCuts) {
                return;
            }
            KitchenObjectService.Instance.Destroy(counterObject);
            KitchenObjectService.Instance.Spawn(recipe.output, this);
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
