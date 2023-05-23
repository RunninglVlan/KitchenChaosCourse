using System;
using UnityEngine;

namespace KitchenChaos {
    public class MenuPlayers : MonoBehaviour {
        [SerializeField] private PlayerVisual[] visuals = Array.Empty<PlayerVisual>();
        [SerializeField] private PlayerColors playerColors = null!;

        void Awake() {
            for (var i = 0; i < visuals.Length; i++) {
                visuals[i].SetColor(playerColors.Get[i]);
            }
        }
    }
}
