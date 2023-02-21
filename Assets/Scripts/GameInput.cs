using UnityEngine;

public class GameInput : MonoBehaviour {
    public InputActions Actions { get; private set; }

    void Awake() {
        Actions = new InputActions();
        Actions.Enable();
    }
}
