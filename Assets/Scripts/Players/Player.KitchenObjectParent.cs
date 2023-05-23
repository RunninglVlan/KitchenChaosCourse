using System;
using KitchenChaos.KitchenObjects;
using UnityEngine;

namespace KitchenChaos.Players {
    public partial class Player : IKitchenObjectParent {
        public static event Action<Player> PickedUp = delegate { };

        [SerializeField] private Transform hands = null!;

        private KitchenObject? kitchenObject;
        public Transform ObjectLocation => hands;

        public bool TryGetKitchenObject(out KitchenObject foundObject) {
            foundObject = null!;
            if (!kitchenObject) {
                return false;
            }
            foundObject = kitchenObject!;
            return true;
        }

        public void ClearKitchenObject() => kitchenObject = null;

        public void SetKitchenObject(KitchenObject value) {
            kitchenObject = value;
            PickedUp(this);
        }

        public bool HasKitchenObject() => kitchenObject;
    }
}
