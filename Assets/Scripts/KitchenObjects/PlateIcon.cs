using UnityEngine;
using UnityEngine.UI;

namespace KitchenObjects {
    public class PlateIcon : MonoBehaviour {
        [SerializeField] private Image image = null!;

        public Sprite Sprite {
            set => image.sprite = value;
        }
    }
}
