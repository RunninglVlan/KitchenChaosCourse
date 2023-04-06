using KitchenObjects;
using UnityEngine;

public interface IKitchenObjectParent {
    Transform ObjectLocation { get; }
    bool TryGetKitchenObject(out KitchenObject foundObject);
    void ClearKitchenObject();
    void SetKitchenObject(KitchenObject value);
    bool HasKitchenObject();
}
