using Unity.Netcode.Components;

namespace KitchenChaos {
    public class OwnerNetworkAnimator : NetworkAnimator {
        protected override bool OnIsServerAuthoritative() {
            return false;
        }
    }
}
