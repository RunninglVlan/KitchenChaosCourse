﻿using System;
using System.Diagnostics.CodeAnalysis;
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

        public void Destroy(KitchenObject kitchenObject) {
            DestroyServerRpc(kitchenObject.NetworkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void DestroyServerRpc(NetworkObjectReference networkObjectReference) {
            ClearKitchenObjectOnParentClientRpc(networkObjectReference);
            networkObjectReference.TryGet(out var networkObject);
            var kitchenObject = networkObject.GetComponent<KitchenObject>();
            Destroy(kitchenObject.gameObject);
        }

        [ClientRpc]
        [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local", Justification = "Rpc can't be static")]
        private void ClearKitchenObjectOnParentClientRpc(NetworkObjectReference networkObjectReference) {
            networkObjectReference.TryGet(out var networkObject);
            var kitchenObject = networkObject.GetComponent<KitchenObject>();
            kitchenObject.ClearKitchenObjectOnParent();
        }
    }
}