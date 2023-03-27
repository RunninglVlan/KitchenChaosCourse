using UnityEngine;

public class ContainerCounter : Counter {
    [SerializeField] private KitchenObjectScriptable kitchenObjectScriptable = null!;
    [SerializeField] private ContainerCounterVisual visual = null!;

    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            return;
        }
        var instance = Instantiate(kitchenObjectScriptable.prefab);
        instance.Parent = player;
        visual.Open();
    }
}
