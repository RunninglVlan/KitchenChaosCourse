using Counters;
using UnityEngine;

namespace Services {
    public class SoundService : MonoBehaviour {
        private const string VOLUME = "SoundVolume";

        [SerializeField] private Sounds sounds = null!;

        private float baseVolume;

        public float Volume {
            get => PlayerPrefs.GetFloat(VOLUME, 1);
            set {
                baseVolume = value;
                PlayerPrefs.SetFloat(VOLUME, value);
            }
        }

        public static SoundService Instance { get; private set; } = null!;

        void Awake() {
            if (Instance) {
                Debug.LogError("Multiple instances in the scene");
            }
            Instance = this;
            baseVolume = Volume;
        }

        void Start() {
            DeliveryService.Instance.DeliverySucceeded += PlayDeliverySuccess;
            DeliveryService.Instance.DeliveryFailed += PlayDeliveryFail;
            Player.Instance.PickedUp += PlayPickup;
        }

        private void PlayDeliverySuccess() {
            Play(sounds.deliverySuccess, DeliveryCounter.Instance.transform.position);
        }

        private void PlayDeliveryFail() {
            Play(sounds.deliveryFail, DeliveryCounter.Instance.transform.position);
        }

        public void PlayChop(CuttingCounter counter) {
            Play(sounds.chop, counter.transform.position);
        }

        private void PlayPickup() {
            Play(sounds.objectPickup, Player.Instance.transform.position);
        }

        public void PlayDrop(Counter counter) {
            Play(sounds.objectDrop, counter.transform.position);
        }

        public void PlayTrash(TrashCounter counter) {
            Play(sounds.trash, counter.transform.position);
        }

        public void PlayFootstep(Vector3 position, float volume = 1) {
            Play(sounds.footstep, position, volume);
        }

        public void PlayWarning() => Play(sounds.warning, Vector3.zero);

        private void Play(AudioClip[] clips, Vector3 position, float volume = 1) {
            Play(clips.GetRandom(), position, volume);
        }

        private void Play(AudioClip clip, Vector3 position, float volume = 1) {
            AudioSource.PlayClipAtPoint(clip, position, baseVolume * volume);
        }
    }
}
