using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos {
    public class PlayerInit : NetworkBehaviour {
        [SerializeField] private PlayerVisual visual = null!;

        private bool set;

        void Start() {
            PlayerInitService.Instance.RequestData();
        }

        public void Init(ulong objectId, Color color, Vector2 position) {
            if (OwnerClientId != objectId || set) {
                return;
            }
            set = true;
            visual.SetColor(color);
            transform.position = new Vector3(position.x, 0, position.y);
        }
    }
}
