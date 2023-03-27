public class ClearCounter : Counter {
    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            player.GetKitchenObject().Parent = this;
        } else if (HasKitchenObject()) {
            GetKitchenObject().Parent = player;
        }
    }
}
