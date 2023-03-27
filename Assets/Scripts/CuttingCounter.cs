using System;
using UnityEngine;

public class CuttingCounter : Counter {
    [SerializeField] private KitchenObjectRecipe[] recipes = Array.Empty<KitchenObjectRecipe>();

    public override void Interact(Player player) {
        var counterHasObject = TryGetKitchenObject(out var counterObject);
        if (player.TryGetKitchenObject(out var playerObject)) {
            if (counterHasObject || !TryGetOutput(playerObject.Scriptable, out _)) {
                return;
            }
            playerObject.Parent = this;
        } else if (counterHasObject) {
            counterObject.Parent = player;
        }
    }

    public override void InteractAlternate() {
        if (!TryGetKitchenObject(out var counterObject)) {
            return;
        }
        if (!TryGetOutput(counterObject.Scriptable, out var cutObject)) {
            return;
        }
        counterObject.DestroySelf();
        KitchenObject.Spawn(cutObject, this);
    }

    private bool TryGetOutput(KitchenObjectScriptable input, out KitchenObjectScriptable output) {
        output = null!;
        foreach (var recipe in recipes) {
            if (recipe.input != input) {
                continue;
            }
            output = recipe.output;
            return true;
        }
        return false;
    }
}
