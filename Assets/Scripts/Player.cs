using UnityEngine;

public class Player : MonoBehaviour {
    private const float ROTATE_SPEED = 10;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private float speed = 1;

    public bool IsWalking { get; private set; }

    void Update() {
        var input = gameInput.Actions.Player.Move.ReadValue<Vector2>().normalized;

        var move = new Vector3(input.x, 0, input.y);
        var playerTransform = transform;
        playerTransform.position += move * (speed * Time.deltaTime);
        var forward = Vector3.Slerp(playerTransform.forward, move, ROTATE_SPEED * Time.deltaTime);
        if (forward != Vector3.zero) {
            playerTransform.forward = forward;
        }

        IsWalking = move != Vector3.zero;
    }
}
