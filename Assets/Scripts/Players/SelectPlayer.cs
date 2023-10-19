using KitchenChaos.Services;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace KitchenChaos.Players {
    public class SelectPlayer : MonoBehaviour {
        [SerializeField] private int index;
        [SerializeField] private TextMeshPro playerName = null!;
        [SerializeField] private TextMeshPro ready = null!;
        [SerializeField] private PlayerVisual visual = null!;
        [SerializeField] private Button kick = null!;

        void Awake() {
            kick.onClick.AddListener(() => {
                var data = NetworkService.Instance.PlayerData(index);
                NetworkLobby.Instance.KickPlayer(data.playerId.ToString());
                NetworkService.Instance.KickPlayer(data.clientId);
            });
        }

        void Start() {
            NetworkService.Instance.OnPlayerDataChanged += UpdatePlayer;
            ReadyService.Instance.PlayerBecameReadyOnClient += UpdatePlayer;
            var hostCharacter = NetworkService.Instance.IsPlayerConnected(index);
            kick.gameObject.SetActive(NetworkManager.Singleton.IsServer && !hostCharacter);
            UpdatePlayer();
        }

        private void UpdatePlayer() {
            var isConnected = NetworkService.Instance.IsPlayerConnected(index);
            gameObject.SetActive(isConnected);
            if (!isConnected) {
                return;
            }
            var data = NetworkService.Instance.PlayerData(index);
            playerName.text = data.name.ToString();
            ready.gameObject.SetActive(ReadyService.Instance.IsPlayerReady(data.clientId));
            visual.SetColor(NetworkService.Instance.PlayerColor(data.colorIndex));
        }

        void OnDestroy() {
            NetworkService.Instance.OnPlayerDataChanged -= UpdatePlayer;
        }
    }
}
