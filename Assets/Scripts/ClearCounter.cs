using UnityEngine;

public class ClearCounter : Counter {
    [SerializeField] private KitchenObjectScriptable kitchenObjectScriptable = null!;

    public override void Interact(Player player) {
        if (HasKitchenObject()) {
            var instance = Instantiate(kitchenObjectScriptable.prefab);
            instance.Parent = this;
        } else {
            GetKitchenObject().Parent = player;
        }
    }
}
