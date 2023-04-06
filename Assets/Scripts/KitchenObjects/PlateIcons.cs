using UnityEngine;

namespace KitchenChaos.KitchenObjects {
    public class PlateIcons : MonoBehaviour {
        [SerializeField] private PlateObject plateObject = null!;
        [SerializeField] private PlateIcon iconTemplate = null!;

        void Awake() {
            plateObject.IngredientAdded += ShowIngredients;
        }

        private void ShowIngredients(KitchenObjectScriptable _) {
            foreach (Transform child in transform) {
                if (child == iconTemplate.transform) {
                    continue;
                }
                Destroy(child.gameObject);
            }
            foreach (var ingredient in plateObject.Ingredients) {
                var icon = Instantiate(iconTemplate, transform);
                icon.gameObject.SetActive(true);
                icon.Sprite = ingredient.sprite;
            }
        }
    }
}
