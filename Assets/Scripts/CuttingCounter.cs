using UnityEngine;

public class CuttingCounter : Counter {
    [SerializeField] private KitchenObjectScriptable cutObjectScriptable = null!;

    public override void Interact(Player player) {
        var counterHasObject = TryGetKitchenObject(out var counterObject);
        if (player.TryGetKitchenObject(out var playerObject)) {
            if (counterHasObject) {
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
        counterObject.DestroySelf();
        KitchenObject.Spawn(cutObjectScriptable, this);
    }
}
