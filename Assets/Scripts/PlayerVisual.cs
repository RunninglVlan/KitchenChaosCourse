using UnityEngine;

namespace KitchenChaos {
    public class PlayerVisual : MonoBehaviour {
        [SerializeField] private MeshRenderer head = null!;
        [SerializeField] private MeshRenderer body = null!;

        public void SetColor(Color color) {
            var material = head.material;
            material.color = color;
            head.material = material;
            body.material = material;
        }
    }
}
