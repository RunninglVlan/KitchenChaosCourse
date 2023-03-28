using KitchenObjects;
using UnityEngine;

namespace Counters {
    public class ContainerCounter : Counter {
        [SerializeField] private KitchenObjectScriptable containerObject = null!;
        [SerializeField] private ContainerCounterVisual visual = null!;

        public override void Interact(Player player) {
            if (player.HasKitchenObject()) {
                return;
            }
            KitchenObject.Spawn(containerObject, player);
            visual.Open();
        }
    }
}
