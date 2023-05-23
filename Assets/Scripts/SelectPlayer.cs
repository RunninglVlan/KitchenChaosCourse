using KitchenChaos.Services;
using TMPro;
using UnityEngine;

namespace KitchenChaos {
    public class SelectPlayer : MonoBehaviour {
        [SerializeField] private int index;
        [SerializeField] private TextMeshPro ready = null!;

        void Start() {
            NetworkService.Instance.OnPlayerDataChanged += UpdatePlayer;
            ReadyService.Instance.PlayerBecameReadyOnClient += UpdatePlayer;
            UpdatePlayer();
        }

        private void UpdatePlayer() {
            var isConnected = NetworkService.Instance.IsPlayerConnected(index);
            gameObject.SetActive(isConnected);
            if (!isConnected) {
                return;
            }
            var data = NetworkService.Instance.PlayerData(index);
            ready.gameObject.SetActive(ReadyService.Instance.IsPlayerReady(data.clientId));
        }

        void OnDestroy() {
            NetworkService.Instance.OnPlayerDataChanged -= UpdatePlayer;
            ReadyService.Instance.PlayerBecameReadyOnClient -= UpdatePlayer;
        }
    }
}
