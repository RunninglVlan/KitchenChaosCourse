namespace Counters {
    public class ClearCounter : Counter {
        public override void Interact(Player player) {
            var counterHasObject = TryGetKitchenObject(out var counterObject);
            if (player.TryGetKitchenObject(out var playerObject)) {
                if (counterHasObject) {
                    ProcessBothHaveObjects();
                    return;
                }
                playerObject.Parent = this;
            } else if (counterHasObject) {
                counterObject.Parent = player;
            }

            void ProcessBothHaveObjects() {
                if (TryAddToPlate(playerObject, counterObject)) {
                    return;
                }
                TryAddToPlate(counterObject, playerObject);
            }
        }
    }
}
