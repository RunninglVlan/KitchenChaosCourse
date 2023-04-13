using KitchenChaos.KitchenObjects;
using KitchenChaos.Services;
using UnityEngine;

namespace KitchenChaos.Counters {
    public class ContainerCounter : Counter {
        [SerializeField] private KitchenObjectScriptable containerObject = null!;
        [SerializeField] private ContainerCounterVisual visual = null!;

        public override void Interact(Player player) {
            if (player.HasKitchenObject()) {
                return;
            }
            KitchenObjectService.Instance.Spawn(containerObject, player);
            visual.Open();
        }
    }
}
