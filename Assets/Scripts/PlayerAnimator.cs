using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    private static readonly int IS_WALKING = Animator.StringToHash("IsWalking");

    [SerializeField] private Player player;

    private Animator animator;

    void Awake() => animator = GetComponent<Animator>();

    void Update() => animator.SetBool(IS_WALKING, player.IsWalking);
}
