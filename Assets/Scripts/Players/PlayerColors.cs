using System;
using UnityEngine;

namespace KitchenChaos.Players {
    [CreateAssetMenu(fileName = nameof(PlayerColors), menuName = "Scriptable/" + nameof(PlayerColors))]
    public class PlayerColors : ScriptableObject {
        [SerializeField] private Color[] colors = Array.Empty<Color>();

        public Color[] Get => colors;
    }
}
