using UnityEngine;

public class PostMusic : MonoBehaviour {
    public AK.Wwise.Event MusicEvent;

    void Start() {
        MusicEvent.Post(
            gameObject,
            (uint)AkCallbackType.AK_MusicSyncBeat,
            MusicCallback
        );
    }

    void MusicCallback(object in_cookie, AkCallbackType in_type, object in_info) {
        GameBehavior.PushRhythmAction();
    }
}
