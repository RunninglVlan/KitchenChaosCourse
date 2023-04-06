using System;
using JetBrains.Annotations;
using KitchenChaos.KitchenObjects;

namespace KitchenChaos.Counters {
    [Serializable]
    public class CuttingRecipe {
        [UsedImplicitly] public string name = null!;
        public KitchenObjectScriptable input = null!;
        public KitchenObjectScriptable output = null!;
        public int maxCuts = 3;
    }

    [Serializable]
    public class StoveRecipe {
        [UsedImplicitly] public string name = null!;
        public KitchenObjectScriptable input = null!;
        public KitchenObjectScriptable output = null!;
        public float maxSeconds = 3;
    }
}
