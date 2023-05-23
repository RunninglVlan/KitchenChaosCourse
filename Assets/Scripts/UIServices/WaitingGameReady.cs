using KitchenChaos.Services;

namespace KitchenChaos.UIServices {
    public class WaitingGameReady : UIService {
        void Start() {
            Hide();
            GameService.Instance.PlayerBecameReadyOnServer += Show;
            GameService.Instance.StateChanged += HideOnCountdown;

            void HideOnCountdown() {
                if (GameService.Instance.IsCountingDownToStart) {
                    Hide();
                }
            }
        }
    }
}
