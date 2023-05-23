using System.Collections.Generic;
using UnityEngine;

namespace KitchenChaos.Players {
    [CreateAssetMenu(fileName = nameof(PlayerColors), menuName = "Scriptable/" + nameof(PlayerColors))]
    public class PlayerColors : ScriptableObject {
        [SerializeField] private List<Color> colors = null!;

        public IReadOnlyList<Color> Get => colors.AsReadOnly();
    }
}
