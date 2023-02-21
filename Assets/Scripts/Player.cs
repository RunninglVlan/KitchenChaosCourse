using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    [SerializeField] private float speed = 1;

    void Update() {
        var input = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) {
            input.y++;
        }
        if (Keyboard.current.sKey.isPressed) {
            input.y--;
        }
        if (Keyboard.current.aKey.isPressed) {
            input.x--;
        }
        if (Keyboard.current.dKey.isPressed) {
            input.x++;
        }
        input = input.normalized;

        var move = new Vector3(input.x, 0, input.y);
        transform.position += move * (speed * Time.deltaTime);
    }
}
