using KitchenChaos.Services;

namespace KitchenChaos.UIServices {
    public class WaitingReady : UIService {
        void Start() {
            Hide();
            ReadyService.Instance.PlayerBecameReadyOnServer += Show;
        }
    }
}
