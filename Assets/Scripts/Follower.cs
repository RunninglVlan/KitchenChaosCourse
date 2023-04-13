using UnityEngine;

namespace KitchenChaos {
    public class Follower : MonoBehaviour {
        public Transform? Target { private get; set; }

        void LateUpdate() {
            if (!Target) {
                return;
            }
            var objectTransform = transform;
            objectTransform.position = Target!.position;
            objectTransform.rotation = Target!.rotation;
        }
    }
}
