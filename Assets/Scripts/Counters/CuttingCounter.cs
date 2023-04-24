using System;
using KitchenChaos.KitchenObjects;
using KitchenChaos.Services;
using Unity.Netcode;
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
                PlaceServerRpc();
            } else if (counterHasObject && canTake) {
                counterObject.Parent = player;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void PlaceServerRpc() {
            PlaceClientRpc();
        }

        [ClientRpc]
        private void PlaceClientRpc() {
            cuts = 0;
            ProgressSet(progress = 0);
        }

        public override void InteractAlternate() {
            CutServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void CutServerRpc() {
            if (!TryGetKitchenObject(out var counterObject)) {
                return;
            }
            if (!TryGetRecipe(counterObject.Scriptable, out var recipe)) {
                return;
            }
            CutClientRpc(recipe.maxCuts);
            if (cuts < recipe.maxCuts) {
                return;
            }
            KitchenObjectService.Instance.Destroy(counterObject);
            KitchenObjectService.Instance.Spawn(recipe.output, this);
        }

        [ClientRpc]
        private void CutClientRpc(int maxCuts) {
            cuts++;
            ProgressSet(progress = (float)cuts / maxCuts);
            visual.Cut();
            SoundService.Instance.PlayChop(this);
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
