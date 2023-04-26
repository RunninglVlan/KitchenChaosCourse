using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KitchenChaos.Services {
    public class GameService : NetworkSingleton<GameService> {
        private const float MAX_COUNTDOWN_SECONDS = 3;
        public const float MAX_PLAYING_SECONDS = 60;

        public event Action PlayerBecameReady = delegate { };
        public event Action StateChanged = delegate { };
        public event Action Paused = delegate { };
        public event Action Unpaused = delegate { };

        private readonly NetworkVariable<State> state = new();
        private bool localPlayerReady;
        private readonly List<ulong> playerReadyStates = new();
        private readonly NetworkVariable<float> seconds = new();
        public bool IsPlaying => state.Value == State.GamePlaying;
        public bool IsCountingDownToStart => state.Value == State.CountdownToStart;
        public bool IsGameOver => state.Value == State.GameOver;
        public int PlayingSeconds => IsPlaying ? (int)MAX_PLAYING_SECONDS - Mathf.CeilToInt(seconds.Value) : 0;
        private bool paused;

        public int CountdownSeconds =>
            IsCountingDownToStart ? (int)MAX_COUNTDOWN_SECONDS - Mathf.FloorToInt(seconds.Value) : 0;

        public override void OnNetworkSpawn() {
            state.OnValueChanged += TriggerStateChanged;

            void TriggerStateChanged(State _, State __) {
                StateChanged();
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
                PlayerBecameReady();
                SetPlayerReadyServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetPlayerReadyServerRpc(ServerRpcParams parameters = default) {
            playerReadyStates.Add(parameters.Receive.SenderClientId);
            foreach (var client in NetworkManager.Singleton.ConnectedClientsIds) {
                if (!playerReadyStates.Contains(client)) {
                    return;
                }
            }
            state.Value = State.CountdownToStart;
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
            paused = !paused;
            Time.timeScale = paused ? 0 : 1;
            if (paused) {
                Paused();
            } else {
                Unpaused();
            }
        }

        private enum State {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameOver
        }
    }
}
