using UnityEngine;
using UnityEngine.UIElements;

namespace Services {
    public abstract class UIService : MonoBehaviour {
        [SerializeField] protected UIDocument document = null!;
    }
}
