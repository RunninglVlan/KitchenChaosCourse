using System;
using JetBrains.Annotations;
using KitchenObjects;

[Serializable]
public class DeliveryRecipe {
    [UsedImplicitly] public string name = null!;
    public KitchenObjectScriptable[] ingredients = Array.Empty<KitchenObjectScriptable>();
}
