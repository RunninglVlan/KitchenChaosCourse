using KitchenChaos.KitchenObjects;
using KitchenChaos.Services;
using Unity.Netcode;
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
            OpenServerRpc();
        }

        [ServerRpc(RequireOwnership = false)]
        private void OpenServerRpc() {
            OpenClientRpc();
        }

        [ClientRpc]
        private void OpenClientRpc() {
            visual.Open();
        }
    }
}
