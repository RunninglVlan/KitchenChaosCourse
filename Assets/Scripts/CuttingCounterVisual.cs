using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour {
    private static readonly int CUT = Animator.StringToHash("Cut");

    private Animator animator = null!;

    void Awake() => animator = GetComponent<Animator>();

    public void Cut() => animator.SetTrigger(CUT);
}
