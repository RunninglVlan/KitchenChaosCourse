using System;
using UnityEngine;
using UnityEngine.UI;

public interface IHasProgress {
    event Action<float> ProgressSet;
}

public class ProgressBar : MonoBehaviour {
    [SerializeField] private GameObject hasProgressObject = null!;
    [SerializeField] private Image progress = null!;
    [SerializeField] private Color normalColor = Color.yellow;
    [SerializeField] private Color warningColor = Color.red;

    void Start() {
        var hasProgress = hasProgressObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {
            Debug.LogError($"{hasProgressObject} has to implement {nameof(IHasProgress)} interface");
            return;
        }
        hasProgress.ProgressSet += Set;
        SetColor(ColorType.Normal);
        gameObject.SetActive(false);

        void Set(float value) {
            progress.fillAmount = value;
            gameObject.SetActive(value is > 0 and < 1);
        }
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
