using System;
using KitchenChaos.KitchenObjects;
using UnityEngine;

namespace KitchenChaos.Counters {
    public class StoveCounter : Counter, IHasProgress {
        public event Action<float> ProgressSet = delegate { };
        public event Action<State> StateChanged = delegate { };
        public event Action WarningSet = delegate { };

        [SerializeField] private StoveRecipe[] fryingRecipes = Array.Empty<StoveRecipe>();
        [SerializeField] private StoveRecipe[] burningRecipes = Array.Empty<StoveRecipe>();

        private State state = State.Idle;
        private StoveRecipe currentRecipe = null!;
        private float seconds;
        private bool setWarning;

        public override void Interact(Player player) {
            var counterHasObject = TryGetKitchenObject(out var counterObject);
            if (player.TryGetKitchenObject(out var playerObject)) {
                if (counterHasObject) {
                    ProcessBothHaveObjects();
                    return;
                }
                if (!TryGetRecipe(fryingRecipes, playerObject.Scriptable, out var recipe)) {
                    return;
                }
                playerObject.Parent = this;
                setWarning = false;
                seconds = 0;
                currentRecipe = recipe;
                state = State.Frying;
                StateChanged(state);
            } else if (counterHasObject && state is State.Fried or State.Burned) {
                counterObject.Parent = player;
                GoToIdle();
            }

            void ProcessBothHaveObjects() {
                if (!TryAddToPlate(playerObject, counterObject)) {
                    return;
                }
                GoToIdle();
            }

            void GoToIdle() {
                state = State.Idle;
                StateChanged(state);
                ProgressSet(0);
            }
        }

        void Update() {
            if (!TryGetKitchenObject(out var counterObject)) {
                return;
            }
            switch (state) {
                case State.Frying:
                    Fry(State.Fried, burningRecipes);
                    break;
                case State.Fried:
                    Fry(State.Burned);
                    var progress = seconds / currentRecipe.maxSeconds;
                    if (!setWarning && progress > .5f) {
                        setWarning = true;
                        WarningSet();
                    }
                    break;
            }

            void Fry(State nextState, StoveRecipe[]? recipes = null) {
                seconds += Time.deltaTime;
                var progress = seconds / currentRecipe.maxSeconds;
                ProgressSet(progress);
                if (progress < 1) {
                    return;
                }
                counterObject.DestroySelf();
                KitchenObject.Spawn(currentRecipe.output, this);
                seconds = 0;
                state = nextState;
                StateChanged(state);
                if (!TryGetKitchenObject(out counterObject) || recipes == null ||
                    !TryGetRecipe(recipes, counterObject.Scriptable, out var recipe)) {
                    return;
                }
                currentRecipe = recipe;
            }
        }

        private static bool TryGetRecipe(StoveRecipe[] recipes, KitchenObjectScriptable input, out StoveRecipe result) {
            result = null!;
            foreach (var recipe in recipes) {
                if (recipe.input != input) {
                    continue;
                }
                result = recipe;
                return true;
            }
            return false;
        }

        public enum State {
            Idle,
            Frying,
            Fried,
            Burned
        }
    }
}
