using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KitchenChaos.Services {
    public class GameService : MonoSingleton<GameService> {
        private const float MAX_COUNTDOWN_SECONDS = 3;
        public const float MAX_PLAYING_SECONDS = 60;

        public event Action StateChanged = delegate { };
        public event Action Paused = delegate { };
        public event Action Unpaused = delegate { };

        private State state = State.WaitingToStart;
        private float seconds;
        public bool IsPlaying => state == State.GamePlaying;
        public bool IsCountingDownToStart => state == State.CountdownToStart;
        public bool IsGameOver => state == State.GameOver;
        public int PlayingSeconds => IsPlaying ? (int)MAX_PLAYING_SECONDS - Mathf.CeilToInt(seconds) : 0;
        private bool paused;

        public int CountdownSeconds =>
            IsCountingDownToStart ? (int)MAX_COUNTDOWN_SECONDS - Mathf.FloorToInt(seconds) : 0;

        void Start() {
            GameInput.Instance.Actions.Player.Interact.performed += GoToCountdown;
            GameInput.Instance.Actions.Player.Pause.performed += TogglePause;

            void GoToCountdown(InputAction.CallbackContext _) {
                if (state != State.WaitingToStart) {
                    return;
                }
                state = State.CountdownToStart;
                StateChanged();
            }
        }

        void Update() {
            switch (state) {
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
                seconds += Time.deltaTime;
                if (seconds < maxSeconds) {
                    return;
                }
                seconds = 0;
                state = nextState;
                StateChanged();
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
