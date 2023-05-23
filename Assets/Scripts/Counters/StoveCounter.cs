using System;
using KitchenChaos.KitchenObjects;
using KitchenChaos.Players;
using KitchenChaos.Services;
using Unity.Netcode;
using UnityEngine;

namespace KitchenChaos.Counters {
    public class StoveCounter : Counter, IHasProgress {
        public event Action<float> ProgressSet = delegate { };
        public event Action<State> StateChanged = delegate { };
        public event Action WarningSet = delegate { };

        [SerializeField] private StoveRecipe[] fryingRecipes = Array.Empty<StoveRecipe>();
        [SerializeField] private StoveRecipe[] burningRecipes = Array.Empty<StoveRecipe>();

        private readonly NetworkVariable<State> state = new();
        private StoveRecipe currentRecipe = null!;
        private readonly NetworkVariable<float> seconds = new();
        private bool setWarning;

        public override void OnNetworkSpawn() {
            seconds.OnValueChanged += TriggerProgressSet;
            state.OnValueChanged += TriggerStateChanged;

            void TriggerProgressSet(float _, float value) {
                ProgressSet(value / currentRecipe.maxSeconds);
            }

            void TriggerStateChanged(State _, State value) {
                StateChanged(value);
                if (value == State.Idle) {
                    ProgressSet(0);
                }
            }
        }

        public override void Interact(Player player) {
            var counterHasObject = TryGetKitchenObject(out var counterObject);
            if (player.TryGetKitchenObject(out var playerObject)) {
                if (counterHasObject) {
                    ProcessBothHaveObjects();
                    return;
                }
                if (!RecipeExists(fryingRecipes, playerObject.Scriptable)) {
                    return;
                }
                playerObject.Parent = this;
                var scriptableIndex = KitchenObjectService.Instance.GetIndex(playerObject.Scriptable);
                StartFryingServerRpc(scriptableIndex);
            } else if (counterHasObject && state.Value is State.Fried or State.Burned) {
                counterObject.Parent = player;
                GoToIdleServerRpc();
            }

            void ProcessBothHaveObjects() {
                if (!TryAddToPlate(playerObject, counterObject)) {
                    return;
                }
                GoToIdleServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void StartFryingServerRpc(int scriptableIndex) {
            seconds.Value = 0;
            state.Value = State.Frying;
            setWarning = false;
            SetFryingRecipeClientRpc(scriptableIndex);
        }

        [ClientRpc]
        private void SetFryingRecipeClientRpc(int scriptableIndex) {
            var scriptable = KitchenObjectService.Instance.Get(scriptableIndex);
            currentRecipe = GetRecipe(fryingRecipes, scriptable);
        }

        [ServerRpc(RequireOwnership = false)]
        private void GoToIdleServerRpc() {
            state.Value = State.Idle;
        }

        void Update() {
            if (!IsServer) {
                return;
            }
            if (!TryGetKitchenObject(out var counterObject)) {
                return;
            }
            switch (state.Value) {
                case State.Frying:
                    Fry(State.Fried, burningRecipes);
                    break;
                case State.Fried:
                    Fry(State.Burned);
                    var progress = seconds.Value / currentRecipe.maxSeconds;
                    if (!setWarning && progress > .5f) {
                        setWarning = true;
                        TriggerWarningSetClientRpc();
                    }
                    break;
            }

            void Fry(State nextState, StoveRecipe[]? recipes = null) {
                seconds.Value += Time.deltaTime;
                var progress = seconds.Value / currentRecipe.maxSeconds;
                if (progress < 1) {
                    return;
                }
                KitchenObjectService.Instance.Destroy(counterObject);
                KitchenObjectService.Instance.Spawn(currentRecipe.output, this);
                seconds.Value = 0;
                state.Value = nextState;
                if (!TryGetKitchenObject(out counterObject) || recipes == null ||
                    !RecipeExists(recipes, counterObject.Scriptable)) {
                    return;
                }
                var scriptableIndex = KitchenObjectService.Instance.GetIndex(counterObject.Scriptable);
                SetBurningRecipeClientRpc(scriptableIndex);
            }
        }

        [ClientRpc]
        private void TriggerWarningSetClientRpc() => WarningSet();

        [ClientRpc]
        private void SetBurningRecipeClientRpc(int scriptableIndex) {
            var scriptable = KitchenObjectService.Instance.Get(scriptableIndex);
            currentRecipe = GetRecipe(burningRecipes, scriptable);
        }

        private static bool RecipeExists(StoveRecipe[] recipes, KitchenObjectScriptable input) {
            foreach (var recipe in recipes) {
                if (recipe.input == input) {
                    return true;
                }
            }
            return false;
        }

        private static StoveRecipe GetRecipe(StoveRecipe[] recipes, KitchenObjectScriptable input) {
            foreach (var recipe in recipes) {
                if (recipe.input == input) {
                    return recipe;
                }
            }
            throw new ArgumentException($"No recipe for {input.name}");
        }

        public enum State {
            Idle,
            Frying,
            Fried,
            Burned
        }
    }
}
