using UnityEngine;

public class GameInput : MonoBehaviour {
    public InputActions Actions { get; private set; } = null!;

    void Awake() {
        Actions = new InputActions();
        Actions.Enable();
    }
}
