public class ClearCounter : Counter {
    public override void Interact(Player player) {
        if (player.TryGetKitchenObject(out var playerObject)) {
            playerObject.Parent = this;
        } else if (TryGetKitchenObject(out var counterObject)) {
            counterObject.Parent = player;
        }
    }
}
