using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace KitchenChaos.Services {
    public class GameService : BaseReadyService<GameService> {
        private const float MAX_COUNTDOWN_SECONDS = 3;
        public const float MAX_PLAYING_SECONDS = 60;

        [SerializeField] private GameObject playerPrefab = null!;

        public sealed override event Action PlayerBecameReadyOnServer = delegate { };
        public event Action StateChanged = delegate { };
        public event Action LocalPaused = delegate { };
        public event Action LocalUnpaused = delegate { };
        public event Action GlobalPaused = delegate { };
        public event Action GlobalUnpaused = delegate { };

        protected override Action ReadyAction => () => state.Value = State.CountdownToStart;

        private readonly NetworkVariable<State> state = new();
        private bool localPlayerReady;
        private readonly Dictionary<ulong, bool> pausedPlayers = new();
        private readonly NetworkVariable<float> seconds = new();
        public bool IsWaitingToStart => state.Value == State.WaitingToStart;
        public bool IsPlaying => state.Value == State.GamePlaying;
        public bool IsCountingDownToStart => state.Value == State.CountdownToStart;
        public bool IsGameOver => state.Value == State.GameOver;
        public int PlayingSeconds => IsPlaying ? (int)MAX_PLAYING_SECONDS - Mathf.CeilToInt(seconds.Value) : 0;
        public bool PausedLocally { get; private set; }
        private readonly NetworkVariable<bool> pausedGlobally = new();
        public bool PausedGlobally => pausedGlobally.Value;

        public int CountdownSeconds =>
            IsCountingDownToStart ? (int)MAX_COUNTDOWN_SECONDS - Mathf.FloorToInt(seconds.Value) : 0;

        public override void OnNetworkSpawn() {
            state.OnValueChanged += TriggerStateChanged;
            pausedGlobally.OnValueChanged += OnPausedChanged;
            if (IsServer) {
                NetworkManager.Singleton.OnClientDisconnectCallback += Unpause;
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SpawnPlayers;
            }

            void TriggerStateChanged(State _, State __) {
                StateChanged();
            }

            void OnPausedChanged(bool _, bool value) {
                Time.timeScale = value ? 0 : 1;
                if (value) {
                    GlobalPaused();
                } else {
                    GlobalUnpaused();
                }
            }
        }

        public override void OnDestroy() {
            base.OnDestroy();
            if (NetworkManager.Singleton && IsServer) {
                NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= SpawnPlayers;
            }
        }

        private void SpawnPlayers(string sceneName, LoadSceneMode _, List<ulong> __, List<ulong> ___) {
            if (SceneService.Instance.IsLoading(sceneName)) {
                return;
            }
            foreach (var client in NetworkManager.Singleton.ConnectedClientsIds) {
                var player = Instantiate(playerPrefab);
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(client, true);
            }
        }

        void Start() {
            GameInput.Instance.Actions.Player.Interact.performed += ReadyPlayer;
            GameInput.Instance.Actions.Player.Pause.performed += TogglePause;

            void ReadyPlayer(InputAction.CallbackContext _) {
                if (state.Value != State.WaitingToStart || localPlayerReady) {
                    return;
                }
                localPlayerReady = true;
                PlayerBecameReadyOnServer();
                SetPlayerReadyServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams parameters = default) {
            BaseSetPlayerReady(parameters.Receive.SenderClientId);
        }

        void Update() {
            if (!IsServer) {
                return;
            }
            switch (state.Value) {
                case State.WaitingToStart:
                    break;
                case State.CountdownToStart:
                    Count(State.GamePlaying, MAX_COUNTDOWN_SECONDS);
                    break;
                case State.GamePlaying:
                    Count(State.GameOver, MAX_PLAYING_SECONDS);
                    break;
                case State.GameOver:
                    break;
            }

            void Count(State nextState, float maxSeconds) {
                seconds.Value += Time.deltaTime;
                if (seconds.Value < maxSeconds) {
                    return;
                }
                seconds.Value = 0;
                state.Value = nextState;
            }
        }

        private void TogglePause(InputAction.CallbackContext _) => TogglePause();

        public void TogglePause() {
            if (IsGameOver) {
                return;
            }
            PausedLocally = !PausedLocally;
            if (PausedLocally) {
                PauseServerRpc();
                LocalPaused();
            } else {
                UnpauseServerRpc();
                LocalUnpaused();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void PauseServerRpc(ServerRpcParams parameters = default) {
            pausedPlayers[parameters.Receive.SenderClientId] = true;
            SetPaused();
        }

        [ServerRpc(RequireOwnership = false)]
        private void UnpauseServerRpc(ServerRpcParams parameters = default) {
            pausedPlayers[parameters.Receive.SenderClientId] = false;
            SetPaused();
        }

        private void Unpause(ulong client) {
            pausedPlayers[client] = false;
            SetPaused();
        }

        private void SetPaused() {
            foreach (var client in NetworkManager.Singleton.ConnectedClientsIds) {
                if (!pausedPlayers.TryGetValue(client, out var value) || !value) {
                    continue;
                }
                pausedGlobally.Value = true;
                return;
            }
            pausedGlobally.Value = false;
        }

        public void ReloadGame() => ReloadGameServerRpc();

        [ServerRpc(RequireOwnership = false)]
        [SuppressMessage("ReSharper", "MemberCanBeMadeStatic.Local", Justification = "Rpc can't be static")]
        private void ReloadGameServerRpc() {
            SceneService.Instance.LoadGame();
        }

        private enum State {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameOver
        }
    }
}
