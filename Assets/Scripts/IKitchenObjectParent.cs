using KitchenChaos.KitchenObjects;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos {
    public interface IKitchenObjectParent {
        Transform ObjectLocation { get; }
        NetworkObject NetworkObject { get; }
        bool TryGetKitchenObject(out KitchenObject foundObject);
        void ClearKitchenObject();
        void SetKitchenObject(KitchenObject value);
        bool HasKitchenObject();
    }
}
