using System;
using System.Collections.Generic;
using KitchenObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour {
    private const float MAX_RECIPE_SECONDS = 4;
    private const float MAX_ORDERS = 4;

    [SerializeField] private DeliveryRecipe[] recipes = Array.Empty<DeliveryRecipe>();
    [SerializeField] private DeliveryManagerUI ui = null!;

    private readonly List<DeliveryRecipe> orders = new();
    private float recipeSeconds;

    public static DeliveryManager Instance { get; private set; } = null!;

    void Awake() {
        if (Instance) {
            Debug.LogError("Multiple instances in the scene");
        }
        Instance = this;
    }

    void Update() {
        if (orders.Count >= MAX_ORDERS) {
            return;
        }
        recipeSeconds += Time.deltaTime;
        if (recipeSeconds < MAX_RECIPE_SECONDS) {
            return;
        }
        recipeSeconds = 0;
        var newRecipe = recipes[Random.Range(0, recipes.Length)];
        orders.Add(newRecipe);
        ui.ShowOrders(orders.AsReadOnly());
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
            ui.ShowOrders(orders.AsReadOnly());
            return;
        }
        Debug.Log("Incorrect plate content");

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
