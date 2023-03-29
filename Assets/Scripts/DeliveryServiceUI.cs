using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeliveryServiceUI : UIService {
    [SerializeField] private VisualTreeAsset orderAsset = null!;

    private VisualElement orders = null!;

    void Start() {
        orders = document.rootVisualElement.Q<VisualElement>("orders");
    }

    public void ShowOrders(IReadOnlyList<DeliveryRecipe> orderedRecipes) {
        for (var index = orders.childCount - 1; index >= 0; index--) {
            orders.RemoveAt(index);
        }
        foreach (var orderedRecipe in orderedRecipes) {
            var order = orderAsset.Instantiate();
            orders.Add(order);
            SetOrder(order, orderedRecipe);
        }

        void SetOrder(VisualElement order, DeliveryRecipe recipe) {
            order.Q<Label>().text = recipe.name;
            var ingredients = order.Q<VisualElement>("ingredients");
            foreach (var ingredient in recipe.ingredients) {
                ingredients.Add(new VisualElement {
                    style = {
                        width = 40,
                        height = 40,
                        backgroundImage = new StyleBackground(ingredient.sprite)
                    }
                });
            }
        }
    }
}
