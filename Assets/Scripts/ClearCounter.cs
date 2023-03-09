using UnityEngine;

public class ClearCounter : MonoBehaviour {
    [SerializeField] private KitchenObjectScriptable kitchenObjectScriptable = null!;
    [SerializeField] private Transform top = null!;

    public void Interact() {
        var kitchenObject = Instantiate(kitchenObjectScriptable.prefab, top);
        Debug.Log(kitchenObject.GetComponent<KitchenObject>().Scriptable.objectName);
    }
}
