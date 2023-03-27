using UnityEngine;

public class ClearCounter : KitchenObjectParent {
    [SerializeField] private KitchenObjectScriptable kitchenObjectScriptable = null!;
    [SerializeField] private Transform top = null!;

    public override Transform ObjectLocation => top;

    public void Interact(Player player) {
        if (kitchenObject == null) {
            var instance = Instantiate(kitchenObjectScriptable.prefab);
            instance.Parent = this;
        } else {
            kitchenObject.Parent = player;
        }
    }
}
