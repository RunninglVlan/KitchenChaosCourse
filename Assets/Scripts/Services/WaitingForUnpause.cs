namespace KitchenChaos.Services {
    public class WaitingForUnpause : UIService {
        void Start() {
            Hide();
            GameService.Instance.LocalPaused += Hide;
            GameService.Instance.LocalUnpaused += OnLocalUnpaused;
            GameService.Instance.GlobalPaused += OnGlobalPaused;
            GameService.Instance.GlobalUnpaused += Hide;

            void OnLocalUnpaused() {
                SetVisible(GameService.Instance.PausedGlobally);
            }

            void OnGlobalPaused() {
                SetVisible(!GameService.Instance.PausedLocally);
            }
        }
    }
}
