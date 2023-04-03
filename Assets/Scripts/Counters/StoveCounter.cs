using System;
using KitchenObjects;
using UnityEngine;

namespace Counters {
    public class StoveCounter : Counter {
        public event Action<State> StateChanged = delegate { };

        [SerializeField] private StoveRecipe[] fryingRecipes = Array.Empty<StoveRecipe>();
        [SerializeField] private StoveRecipe[] burningRecipes = Array.Empty<StoveRecipe>();
        [SerializeField] private ProgressBar progressBar = null!;

        private State state = State.Idle;
        private StoveRecipe currentRecipe = null!;
        private float seconds;

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
                progressBar.SetColor(ProgressBar.ColorType.Normal);
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
                progressBar.Set(0);
                state = State.Idle;
                StateChanged(state);
            }
        }

        void Update() {
            if (!TryGetKitchenObject(out var counterObject)) {
                return;
            }
            switch (state) {
                case State.Frying:
                    Fry(State.Fried, burningRecipes, ProgressBar.ColorType.Warning);
                    break;
                case State.Fried:
                    Fry(State.Burned);
                    break;
            }

            void Fry(State nextState, StoveRecipe[]? recipes = null, ProgressBar.ColorType colorType = default) {
                seconds += Time.deltaTime;
                var progress = seconds / currentRecipe.maxSeconds;
                progressBar.Set(progress);
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
                progressBar.SetColor(colorType);
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
