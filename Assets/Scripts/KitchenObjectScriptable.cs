using UnityEngine;

[CreateAssetMenu(fileName = nameof(KitchenObjectScriptable), menuName = "Scriptable/" + nameof(KitchenObjectScriptable))]
public class KitchenObjectScriptable : ScriptableObject {
    public KitchenObject prefab = null!;
    public Sprite sprite = null!;
    public string objectName = null!;
}
