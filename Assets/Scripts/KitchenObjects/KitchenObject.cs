using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.KitchenObjects {
    public class KitchenObject : NetworkBehaviour {
        [SerializeField] private KitchenObjectScriptable scriptable = null!;

        public KitchenObjectScriptable Scriptable => scriptable;
        private IKitchenObjectParent? parent;
        private Follower follower = null!;

        void Awake() {
            follower = GetComponent<Follower>();
        }

        public IKitchenObjectParent Parent {
            set => SetParentServerRpc(value.NetworkObject);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetParentServerRpc(NetworkObjectReference parentNetworkObjectReference) {
            SetParentClientRpc(parentNetworkObjectReference);
        }

        [ClientRpc]
        private void SetParentClientRpc(NetworkObjectReference parentNetworkObjectReference) {
            parent?.ClearKitchenObject();
            parentNetworkObjectReference.TryGet(out var parentNetworkObject);
            parent = parentNetworkObject.GetComponent<IKitchenObjectParent>();
            if (parent.HasKitchenObject()) {
                Debug.LogError($"Parent already has a {nameof(KitchenObject)}!");
            }
            parent.SetKitchenObject(this);
            follower.Target = parent.ObjectLocation;
        }

        public void ClearKitchenObjectOnParent() {
            parent!.ClearKitchenObject();
        }
    }
}
