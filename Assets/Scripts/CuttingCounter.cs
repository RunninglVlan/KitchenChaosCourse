using System;
using UnityEngine;

public class CuttingCounter : Counter {
    [SerializeField] private KitchenObjectRecipe[] recipes = Array.Empty<KitchenObjectRecipe>();

    private int cuts;

    public override void Interact(Player player) {
        var counterHasObject = TryGetKitchenObject(out var counterObject);
        if (player.TryGetKitchenObject(out var playerObject)) {
            if (counterHasObject || !TryGetRecipe(playerObject.Scriptable, out _)) {
                return;
            }
            playerObject.Parent = this;
            cuts = 0;
        } else if (counterHasObject) {
            counterObject.Parent = player;
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
        if (cuts < recipe.maxCuts) {
            return;
        }
        counterObject.DestroySelf();
        KitchenObject.Spawn(recipe.output, this);
    }

    private bool TryGetRecipe(KitchenObjectScriptable input, out KitchenObjectRecipe result) {
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
