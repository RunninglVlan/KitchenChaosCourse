using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using KitchenObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour {
    private const float MAX_RECIPE_SECONDS = 4;
    private const float MAX_ORDERS = 4;

    [SerializeField] private Recipe[] recipes = Array.Empty<Recipe>();

    private readonly List<Recipe> orderedRecipes = new();
    private float recipeSeconds;

    public static DeliveryManager Instance { get; private set; } = null!;

    void Awake() {
        if (Instance) {
            Debug.LogError("Multiple instances in the scene");
        }
        Instance = this;
    }

    void Update() {
        if (orderedRecipes.Count >= MAX_ORDERS) {
            return;
        }
        recipeSeconds += Time.deltaTime;
        if (recipeSeconds < MAX_RECIPE_SECONDS) {
            return;
        }
        recipeSeconds = 0;
        var newRecipe = recipes[Random.Range(0, recipes.Length)];
        Debug.Log($"New: {newRecipe.name}");
        orderedRecipes.Add(newRecipe);
    }

    public void Deliver(PlateObject plate) {
        for (var index = 0; index < orderedRecipes.Count; index++) {
            var recipe = orderedRecipes[index];
            if (recipe.ingredients.Length != plate.Ingredients.Count) {
                continue;
            }
            if (!PlateContentMatchesRecipe(recipe)) {
                continue;
            }
            orderedRecipes.RemoveAt(index);
            Debug.Log($"Delivered {recipe.name}");
            return;
        }
        Debug.Log("Incorrect plate content");

        bool PlateContentMatchesRecipe(Recipe recipe) {
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

    [Serializable]
    private class Recipe {
        [UsedImplicitly] public string name = null!;
        public KitchenObjectScriptable[] ingredients = Array.Empty<KitchenObjectScriptable>();
    }
}
