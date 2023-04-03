using System;
using UnityEngine;
using UnityEngine.UI;

public interface IHasProgress {
    event Action<float> ProgressSet;
}

public class ProgressBar : MonoBehaviour {
    [SerializeField] private GameObject hasProgressObject = null!;
    [SerializeField] protected Image progress = null!;

    void Start() {
        var hasProgress = hasProgressObject.GetComponent<IHasProgress>();
        if (hasProgress == null) {
            Debug.LogError($"{hasProgressObject} has to implement {nameof(IHasProgress)} interface");
            return;
        }
        hasProgress.ProgressSet += Set;
        gameObject.SetActive(false);

        void Set(float value) {
            progress.fillAmount = value;
            gameObject.SetActive(value is > 0 and < 1);
        }
    }
}
