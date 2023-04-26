using UnityEngine;

namespace KitchenChaos {
    public class PlayerVisual : MonoBehaviour {
        [SerializeField] private MeshRenderer head = null!;
        [SerializeField] private MeshRenderer body = null!;

        public void SetColor(Color color) {
            head.material.color = color;
            body.material.color = color;
        }
    }
}
