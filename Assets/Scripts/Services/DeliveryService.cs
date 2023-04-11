using System;
using System.Collections.Generic;
using KitchenChaos.KitchenObjects;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace KitchenChaos.Services {
    public class DeliveryService : NetworkSingleton<DeliveryService> {
        private const float MAX_RECIPE_SECONDS = 4;
        private const float MAX_ORDERS = 4;

        public event Action DeliverySucceeded = delegate { };
        public event Action DeliveryFailed = delegate { };

        [SerializeField] private DeliveryRecipe[] recipes = Array.Empty<DeliveryRecipe>();

        private readonly List<DeliveryRecipe> orders = new();
        private float recipeSeconds;
        public int DeliveredOrders { get; private set; }

        void Update() {
            if (!IsServer) {
                return;
            }
            if (!GameService.Instance.IsPlaying || orders.Count >= MAX_ORDERS) {
                return;
            }
            recipeSeconds += Time.deltaTime;
            if (recipeSeconds < MAX_RECIPE_SECONDS) {
                return;
            }
            recipeSeconds = 0;
            var randomRecipeIndex = Random.Range(0, recipes.Length);
            SpawnOrderClientRpc(randomRecipeIndex);
        }

        [ClientRpc]
        private void SpawnOrderClientRpc(int index) {
            orders.Add(recipes[index]);
            DeliveryServiceUI.Instance.ShowOrders(orders.AsReadOnly());
        }

        public void Deliver(PlateObject plate) {
            for (var index = 0; index < orders.Count; index++) {
                var order = orders[index];
                if (order.ingredients.Length != plate.Ingredients.Count) {
                    continue;
                }
                if (!PlateContentMatchesOrder(order)) {
                    continue;
                }
                SucceedDeliveryServerRpc(index);
                return;
            }
            FailDeliveryServerRpc();

            bool PlateContentMatchesOrder(DeliveryRecipe order) {
                var result = true;
                foreach (var recipeIngredient in order.ingredients) {
                    var plateHasIngredient = false;
                    foreach (var plateIngredient in plate.Ingredients) {
                        if (recipeIngredient != plateIngredient) {
                            continue;
                        }
                        plateHasIngredient = true;
                        break;
                    }
                    if (!plateHasIngredient) {
                        result = false;
                    }
                }
                return result;
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SucceedDeliveryServerRpc(int orderIndex) {
            SucceedDeliveryClientRpc(orderIndex);
        }

        [ClientRpc]
        private void SucceedDeliveryClientRpc(int orderIndex) {
            orders.RemoveAt(orderIndex);
            DeliveryServiceUI.Instance.ShowOrders(orders.AsReadOnly());
            DeliverySucceeded();
            DeliveredOrders++;
        }

        [ServerRpc(RequireOwnership = false)]
        private void FailDeliveryServerRpc() {
            FailDeliveryClientRpc();
        }

        [ClientRpc]
        private void FailDeliveryClientRpc() {
            DeliveryFailed();
        }
    }
}
