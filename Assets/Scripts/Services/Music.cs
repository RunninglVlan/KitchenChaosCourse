using UnityEngine;

namespace Services {
    public class Music : MonoBehaviour {
        private const string VOLUME = "MusicVolume";

        private AudioSource audioSource = null!;

        public float Volume {
            get => PlayerPrefs.GetFloat(VOLUME, .5f);
            set {
                audioSource.volume = value;
                PlayerPrefs.SetFloat(VOLUME, value);
            }
        }

        public static Music Instance { get; private set; } = null!;

        void Awake() {
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = Volume;
        }
    }
}
