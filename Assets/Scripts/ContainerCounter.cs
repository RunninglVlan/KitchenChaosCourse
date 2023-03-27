using UnityEngine;

public class ContainerCounter : Counter {
    [SerializeField] private KitchenObjectScriptable kitchenObjectScriptable = null!;
    [SerializeField] private ContainerCounterVisual visual = null!;

    public override void Interact(Player player) {
        var instance = Instantiate(kitchenObjectScriptable.prefab);
        instance.Parent = player;
        visual.Open();
    }
}
