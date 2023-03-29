using System;
using UnityEngine;

namespace Services {
    [Serializable]
    public class Sounds {
        public AudioClip[] chop = Array.Empty<AudioClip>();
        public AudioClip[] deliveryFail = Array.Empty<AudioClip>();
        public AudioClip[] deliverySuccess = Array.Empty<AudioClip>();
        public AudioClip[] footstep = Array.Empty<AudioClip>();
        public AudioClip[] objectDrop = Array.Empty<AudioClip>();
        public AudioClip[] objectPickup = Array.Empty<AudioClip>();
        public AudioClip stoveSizzle = null!;
        public AudioClip[] trash = Array.Empty<AudioClip>();
        public AudioClip[] warning = Array.Empty<AudioClip>();
    }
}
