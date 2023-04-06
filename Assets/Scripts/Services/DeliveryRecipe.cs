using System;
using JetBrains.Annotations;
using KitchenChaos.KitchenObjects;

namespace KitchenChaos.Services {
    [Serializable]
    public class DeliveryRecipe {
        [UsedImplicitly] public string name = null!;
        public KitchenObjectScriptable[] ingredients = Array.Empty<KitchenObjectScriptable>();
    }
}
