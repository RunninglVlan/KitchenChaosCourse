using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectScriptable scriptable = null!;

    public KitchenObjectScriptable Scriptable => scriptable;
    private KitchenObjectParent? parent;

    public KitchenObjectParent? Parent {
        get => parent;
        set {
            if (parent != null) {
                parent.ClearKitchenObject();
            }
            parent = value!;
            if (parent.HasKitchenObject()) {
                Debug.LogError($"Parent already has a {nameof(KitchenObject)}!");
            }
            parent.SetKitchenObject(this);
            var objectTransform = transform;
            objectTransform.parent = parent.ObjectLocation;
            objectTransform.localPosition = Vector3.zero;
        }
    }
}
