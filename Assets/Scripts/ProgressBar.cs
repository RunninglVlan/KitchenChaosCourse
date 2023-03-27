using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {
    [SerializeField] private Image progress = null!;
    [SerializeField] private Color normalColor = Color.yellow;
    [SerializeField] private Color warningColor = Color.red;

    void Start() => SetColor(ColorType.Normal);

    public bool IsEmptyOrFilled =>
        Mathf.Approximately(progress.fillAmount, 0) || Mathf.Approximately(progress.fillAmount, 1);

    public void Set(float value) {
        progress.fillAmount = value;
        gameObject.SetActive(value is > 0 and < 1);
    }

    public void SetColor(ColorType type) {
        switch (type) {
            case ColorType.Normal:
                progress.color = normalColor;
                break;
            case ColorType.Warning:
                progress.color = warningColor;
                break;
        }
    }

    public enum ColorType {
        Normal,
        Warning
    }
}
