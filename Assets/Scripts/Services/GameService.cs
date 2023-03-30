using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services {
    public class GameService : MonoBehaviour {
        private const float MAX_WAITING_SECONDS = 1;
        private const float MAX_COUNTDOWN_SECONDS = 3;
        public const float MAX_PLAYING_SECONDS = 60;

        public event Action StateChanged = delegate { };

        private State state = State.WaitingToStart;
        private float seconds;
        public bool IsPlaying => state == State.GamePlaying;
        public bool IsCountingDownToStart => state == State.CountdownToStart;
        public bool IsGameOver => state == State.GameOver;
        public int PlayingSeconds => IsPlaying ? (int) MAX_PLAYING_SECONDS - Mathf.CeilToInt(seconds) : 0;
        public bool IsGamePaused { get; private set; }

        public int CountdownSeconds =>
            IsCountingDownToStart ? (int) MAX_COUNTDOWN_SECONDS - Mathf.FloorToInt(seconds) : 0;

        public static GameService Instance { get; private set; } = null!;

        void Awake() {
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
        }

        void Start() {
            GameInput.Instance.Actions.Player.Pause.performed += TogglePause;
        }

        void Update() {
            switch (state) {
                case State.WaitingToStart:
                    Count(State.CountdownToStart, MAX_WAITING_SECONDS);
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
            IsGamePaused = !IsGamePaused;
            Time.timeScale = IsGamePaused ? 0 : 1;
            StateChanged();
        }

        private enum State {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameOver
        }
    }
}
