using Counters;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField] private Sounds sounds = null!;

    public static SoundManager Instance { get; private set; } = null!;

    void Awake() {
        if (Instance) {
            Debug.LogError("Multiple instances in the scene");
        }
        Instance = this;
    }

    void Start() {
        DeliveryManager.Instance.DeliverySucceeded += PlayDeliverySuccess;
        DeliveryManager.Instance.DeliveryFailed += PlayDeliveryFail;
        CuttingCounter.Cut += PlayChop;
        Player.Instance.PickedUp += PlayPickup;
        Counter.ObjectPlaced += PlayDrop;
        TrashCounter.Trashed += PlayTrash;
    }

    private void PlayDeliverySuccess() {
        Play(sounds.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void PlayDeliveryFail() {
        Play(sounds.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void PlayChop(CuttingCounter counter) {
        Play(sounds.chop, counter.transform.position);
    }

    private void PlayPickup() {
        Play(sounds.objectPickup, Player.Instance.transform.position);
    }

    private void PlayDrop(Counter counter) {
        Play(sounds.objectDrop, counter.transform.position);
    }

    private void PlayTrash(TrashCounter counter) {
        Play(sounds.trash, counter.transform.position);
    }

    public void PlayFootstep(Vector3 position, float volume = 1) {
        Play(sounds.footstep, position, volume);
    }

    private static void Play(AudioClip[] clips, Vector3 position, float volume = 1) {
        Play(clips.GetRandom(), position, volume);
    }

    private static void Play(AudioClip clip, Vector3 position, float volume = 1) {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
}
