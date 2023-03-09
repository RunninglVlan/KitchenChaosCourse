using UnityEngine;

public class KitchenObject : MonoBehaviour {
    [SerializeField] private KitchenObjectScriptable scriptable = null!;

    public KitchenObjectScriptable Scriptable => scriptable;
}
