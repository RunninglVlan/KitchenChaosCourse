using System;
using System.Collections.Generic;
using KitchenChaos.KitchenObjects;
using UnityEngine;

namespace KitchenChaos.Services {
    public class DeliveryService : Singleton<DeliveryService> {
        private const float MAX_RECIPE_SECONDS = 4;
        private const float MAX_ORDERS = 4;

        public event Action DeliverySucceeded = delegate { };
        public event Action DeliveryFailed = delegate { };

        [SerializeField] private DeliveryRecipe[] recipes = Array.Empty<DeliveryRecipe>();

        private readonly List<DeliveryRecipe> orders = new();
        private float recipeSeconds;
        public int DeliveredOrders { get; private set; }

        void Update() {
            if (!GameService.Instance.IsPlaying || orders.Count >= MAX_ORDERS) {
                return;
            }
            recipeSeconds += Time.deltaTime;
            if (recipeSeconds < MAX_RECIPE_SECONDS) {
                return;
            }
            recipeSeconds = 0;
            orders.Add(recipes.GetRandom());
            DeliveryServiceUI.Instance.ShowOrders(orders.AsReadOnly());
        }

        public void Deliver(PlateObject plate) {
            for (var index = 0; index < orders.Count; index++) {
                var recipe = orders[index];
                if (recipe.ingredients.Length != plate.Ingredients.Count) {
                    continue;
                }
                if (!PlateContentMatchesRecipe(recipe)) {
                    continue;
                }
                orders.RemoveAt(index);
                DeliveryServiceUI.Instance.ShowOrders(orders.AsReadOnly());
                DeliverySucceeded();
                DeliveredOrders++;
                return;
            }
            DeliveryFailed();

            bool PlateContentMatchesRecipe(DeliveryRecipe recipe) {
                var result = true;
                foreach (var recipeIngredient in recipe.ingredients) {
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
    }
}
