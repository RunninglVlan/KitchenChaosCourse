using UnityEngine;

namespace Counters {
    public class ContainerCounterVisual : MonoBehaviour {
        private static readonly int OPEN_CLOSE = Animator.StringToHash("OpenClose");

        private Animator animator = null!;

        void Awake() => animator = GetComponent<Animator>();

        public void Open() => animator.SetTrigger(OPEN_CLOSE);
    }
}
