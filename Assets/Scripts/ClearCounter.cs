public class ClearCounter : Counter {
    public override void Interact(Player player) {
        var counterHasObject = TryGetKitchenObject(out var counterObject);
        if (player.TryGetKitchenObject(out var playerObject)) {
            if (counterHasObject) {
                return;
            }
            playerObject.Parent = this;
        } else if (counterHasObject) {
            counterObject.Parent = player;
        }
    }
}
