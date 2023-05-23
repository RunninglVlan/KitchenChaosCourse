using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Players {
    public class PlayerAnimator : NetworkBehaviour {
        private static readonly int IS_WALKING = Animator.StringToHash("IsWalking");

        [SerializeField] private Player player = null!;

        private Animator animator = null!;

        void Awake() => animator = GetComponent<Animator>();

        void Update() {
            if (!IsOwner) {
                return;
            }
            animator.SetBool(IS_WALKING, player.IsWalking);
        }
    }
}
