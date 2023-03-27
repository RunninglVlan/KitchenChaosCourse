using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    [SerializeField] private Image progress = null!;

    public bool IsEmptyOrFilled =>
        Mathf.Approximately(progress.fillAmount, 0) || Mathf.Approximately(progress.fillAmount, 1);

    public void Set(float value) {
        progress.fillAmount = value;
        gameObject.SetActive(value is > 0 and < 1);
    }
}
