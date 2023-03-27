using UnityEngine;

public class ClearCounter : MonoBehaviour {
    [SerializeField] private KitchenObjectScriptable kitchenObjectScriptable = null!;
    [SerializeField] private Transform top = null!;

    private KitchenObject? kitchenObject;
    public Transform Top => top;

    public void ClearKitchenObject() => kitchenObject = null;
    public void SetKitchenObject(KitchenObject value) => kitchenObject = value;
    public bool HasKitchenObject() => kitchenObject != null;

    public void Interact() {
        if (kitchenObject == null) {
            var instance = Instantiate(kitchenObjectScriptable.prefab);
            instance.ClearCounter = this;
        } else {
            Debug.Log(kitchenObject.ClearCounter);
        }
    }
}
