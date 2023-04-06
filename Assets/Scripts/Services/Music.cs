using UnityEngine;

namespace KitchenChaos.Services {
    public class Music : Singleton<Music>  {
        private const string VOLUME = "MusicVolume";

        private AudioSource audioSource = null!;

        public float Volume {
            get => PlayerPrefs.GetFloat(VOLUME, .5f);
            set {
                audioSource.volume = value;
                PlayerPrefs.SetFloat(VOLUME, value);
            }
        }

        protected override void Awake() {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = Volume;
        }
    }
}
