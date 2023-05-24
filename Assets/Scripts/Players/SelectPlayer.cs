using KitchenChaos.Services;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Players {
    public class SelectPlayer : MonoBehaviour {
        [SerializeField] private int index;
        [SerializeField] private TextMeshPro ready = null!;
        [SerializeField] private PlayerVisual visual = null!;
        [SerializeField] private Button kick = null!;

        void Awake() {
            kick.onClick.AddListener(() => {
                var data = NetworkService.Instance.PlayerData(index);
                NetworkService.Instance.KickPlayer(data.clientId);
            });
        }

        void Start() {
            NetworkService.Instance.OnPlayerDataChanged += UpdatePlayer;
            ReadyService.Instance.PlayerBecameReadyOnClient += UpdatePlayer;
            kick.gameObject.SetActive(NetworkManager.Singleton.IsServer);
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
            visual.SetColor(NetworkService.Instance.PlayerColor(data.colorIndex));
        }

        void OnDestroy() {
            NetworkService.Instance.OnPlayerDataChanged -= UpdatePlayer;
        }
    }
}
