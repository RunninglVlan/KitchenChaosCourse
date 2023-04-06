using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    private static readonly int IS_WALKING = Animator.StringToHash("IsWalking");

    [SerializeField] private Player player = null!;

    private Animator animator = null!;

    void Awake() => animator = GetComponent<Animator>();

    void Update() => animator.SetBool(IS_WALKING, player.IsWalking);
}
