using System;
using KitchenChaos.KitchenObjects;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

namespace KitchenChaos.Services {
    public class KitchenObjectService : NetworkSingleton<KitchenObjectService> {
        [SerializeField] private KitchenObjectScriptable[] scriptables = Array.Empty<KitchenObjectScriptable>();

        public void Spawn(KitchenObjectScriptable scriptable, IKitchenObjectParent parent) {
            var scriptableIndex = Array.IndexOf(scriptables, scriptable);
            Assert.IsTrue(scriptableIndex >= 0, $"{scriptable.name} is missing from the list");
            SpawnServerRpc(scriptableIndex, parent.NetworkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnServerRpc(int scriptableIndex, NetworkObjectReference parentNetworkObjectReference) {
            var scriptable = scriptables[scriptableIndex];
            var instance = Instantiate(scriptable.prefab);
            instance.GetComponent<NetworkObject>().Spawn();
            parentNetworkObjectReference.TryGet(out var parentNetworkObject);
            instance.Parent = parentNetworkObject.GetComponent<IKitchenObjectParent>();
        }
    }
}
