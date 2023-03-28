using System.Collections.Generic;
using UnityEngine;

namespace KitchenObjects {
    public class PlateIcons : MonoBehaviour {
        [SerializeField] private PlateIcon iconTemplate = null!;

        public void ShowIngredients(List<KitchenObjectScriptable> ingredients) {
            foreach (Transform child in transform) {
                if (child == iconTemplate.transform) {
                    continue;
                }
                Destroy(child.gameObject);
            }
            foreach (var ingredient in ingredients) {
                var icon = Instantiate(iconTemplate, transform);
                icon.gameObject.SetActive(true);
                icon.Sprite = ingredient.sprite;
            }
        }
    }
}
