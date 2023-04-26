namespace KitchenChaos.Services {
    public class Waiting : UIService {
        void Start() {
            Hide();
            GameService.Instance.PlayerBecameReady += Show;
            GameService.Instance.StateChanged += HideOnCountdown;

            void HideOnCountdown() {
                if (GameService.Instance.IsCountingDownToStart) {
                    Hide();
                }
            }
        }
    }
}
