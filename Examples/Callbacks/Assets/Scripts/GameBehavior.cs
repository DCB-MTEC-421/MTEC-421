using UnityEngine;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;

    // Declaration of Delegate type
    public delegate void OnMusic();

    // Instance of the delegate above
    public static event OnMusic OnMusicAction;

    void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)
            Destroy(Instance);
        else
            Instance = this;
    }

    public static void PushRhythmAction() {
        /*
         * Post Event
         *
         * Make sure not to post event if there are no subscribers.
         * The ?. is the `null` operator. If the expression to the left of the
         * operator is not null, then the action to the right will execute.
         */
        OnMusicAction?.Invoke();
    }
}
