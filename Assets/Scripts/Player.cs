using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {
    private const float ROTATE_SPEED = 10;

    [SerializeField] private float speed = 1;

    public bool IsWalking { get; private set; }

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
        var playerTransform = transform;
        playerTransform.position += move * (speed * Time.deltaTime);
        playerTransform.forward = Vector3.Slerp(playerTransform.forward, move, ROTATE_SPEED * Time.deltaTime);

        IsWalking = move != Vector3.zero;
    }
}
