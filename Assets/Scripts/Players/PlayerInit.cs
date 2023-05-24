using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Players {
    public class PlayerInit : NetworkBehaviour {
        [SerializeField] private PlayerVisual visual = null!;

        private bool set;

        void Start() {
            PlayerInitService.Instance.RequestPosition();
        }

        public void Init(ulong objectId, Vector2 position) {
            if (OwnerClientId != objectId || set) {
                return;
            }
            set = true;
            var data = NetworkService.Instance.PlayerDataFromClientId(OwnerClientId);
            visual.SetColor(NetworkService.Instance.PlayerColor(data.colorIndex));
            transform.position = new Vector3(position.x, 0, position.y);
        }
    }
}
