using UnityEngine;

public abstract class KitchenObjectParent : MonoBehaviour {
    private KitchenObject? kitchenObject;
    public abstract Transform ObjectLocation { get; }

    public void ClearKitchenObject() => kitchenObject = null;
    protected KitchenObject GetKitchenObject() => kitchenObject!;
    public void SetKitchenObject(KitchenObject value) => kitchenObject = value;
    public bool HasKitchenObject() => kitchenObject != null;
}
