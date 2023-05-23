using KitchenChaos.Services;
using UnityEngine;

namespace KitchenChaos.Players {
    public class PlayerSounds : MonoBehaviour {
        private const float MAX_FOOTSTEP_SECONDS = .1f;

        private Player player = null!;
        private float footstepSeconds;

        void Awake() {
            player = GetComponent<Player>();
        }

        void Update() {
            footstepSeconds += Time.deltaTime;
            if (footstepSeconds < MAX_FOOTSTEP_SECONDS) {
                return;
            }
            footstepSeconds = 0;
            if (player.IsWalking) {
                SoundService.Instance.PlayFootstep(transform.position);
            }
        }
    }
}
