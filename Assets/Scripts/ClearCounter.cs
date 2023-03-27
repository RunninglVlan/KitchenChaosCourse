using UnityEngine;

public class ClearCounter : KitchenObjectParent {
    [SerializeField] private KitchenObjectScriptable kitchenObjectScriptable = null!;
    [SerializeField] private Transform top = null!;

    public override Transform ObjectLocation => top;

    public void Interact() {
        if (kitchenObject == null) {
            var instance = Instantiate(kitchenObjectScriptable.prefab);
            instance.Parent = this;
        } else {
            Debug.Log(kitchenObject.Parent);
        }
    }
}
