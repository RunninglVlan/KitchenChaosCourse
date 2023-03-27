using UnityEngine;

namespace Counters {
    public class ContainerCounter : Counter {
        [SerializeField] private KitchenObjectScriptable kitchenObjectScriptable = null!;
        [SerializeField] private ContainerCounterVisual visual = null!;

        public override void Interact(Player player) {
            if (player.HasKitchenObject()) {
                return;
            }
            KitchenObject.Spawn(kitchenObjectScriptable, player);
            visual.Open();
        }
    }
}
