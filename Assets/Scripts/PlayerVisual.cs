using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos {
    public class PlayerVisual : NetworkBehaviour {
        [SerializeField] private MeshRenderer head = null!;
        [SerializeField] private MeshRenderer body = null!;

        void Start() {
            ColorService.Instance.RequestColor();
        }

        public void SetColor(ulong objectId, Color color) {
            if (OwnerClientId != objectId) {
                return;
            }
            head.material.color = color;
            body.material.color = color;
        }
    }
}
