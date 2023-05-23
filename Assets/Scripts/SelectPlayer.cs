using KitchenChaos.Services;
using UnityEngine;

namespace KitchenChaos {
    public class SelectPlayer : MonoBehaviour {
        [SerializeField] private int index;

        void Start() {
            NetworkService.OnPlayerDataChanged += UpdatePlayer;
            UpdatePlayer();

            void UpdatePlayer() {
                gameObject.SetActive(NetworkService.Instance.IsPlayerConnected(index));
            }
        }
    }
}
