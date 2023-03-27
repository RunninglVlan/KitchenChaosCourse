using System;
using JetBrains.Annotations;

[Serializable]
public class KitchenObjectRecipe {
    [UsedImplicitly] public string name = null!;
    public KitchenObjectScriptable input = null!;
    public KitchenObjectScriptable output = null!;
    public int maxCuts = 3;
}
