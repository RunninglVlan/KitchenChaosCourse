using UnityEngine;

public class LookAtCamera : MonoBehaviour {
    [SerializeField] private Mode mode;

    void LateUpdate() {
        switch (mode) {
            case Mode.LookAt:
                transform.LookAt(Camera.main!.transform);
                break;
            case Mode.LookAtInverted:
                var position = transform.position;
                var directionFromCamera = position - Camera.main!.transform.position;
                transform.LookAt(position + directionFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main!.transform.forward;
                break;
            case Mode.CameraForwardInverted:
                transform.forward = -Camera.main!.transform.forward;
                break;
        }
    }

    private enum Mode {
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }
}
