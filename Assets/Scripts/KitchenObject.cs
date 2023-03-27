using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectScriptable scriptable = null!;

    public KitchenObjectScriptable Scriptable => scriptable;
    private ClearCounter? clearCounter;

    public ClearCounter? ClearCounter {
        get => clearCounter;
        set {
            if (clearCounter != null) {
                clearCounter.ClearKitchenObject();
            }
            clearCounter = value!;
            if (clearCounter.HasKitchenObject()) {
                Debug.LogError($"Counter already has a {nameof(KitchenObject)}!");
            }
            clearCounter.SetKitchenObject(this);
            var objectTransform = transform;
            objectTransform.parent = clearCounter.Top;
            objectTransform.localPosition = Vector3.zero;
        }
    }
}
