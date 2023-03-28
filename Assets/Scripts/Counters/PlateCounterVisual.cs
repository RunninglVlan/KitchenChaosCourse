using System.Collections.Generic;
using UnityEngine;

namespace Counters {
    public class PlateCounterVisual : MonoBehaviour {
        private const float PLATE_OFFSET = .1f;

        [SerializeField] private Transform top = null!;
        [SerializeField] private GameObject platePrefab = null!;

        private readonly Stack<GameObject> plates = new();

        public void Spawn() {
            var plate = Instantiate(platePrefab, top);
            plate.transform.localPosition = new Vector3(0, plates.Count * PLATE_OFFSET, 0);
            plates.Push(plate);
        }

        public void DestroyTop() {
            var topPlate = plates.Pop();
            Destroy(topPlate);
        }
    }
}
