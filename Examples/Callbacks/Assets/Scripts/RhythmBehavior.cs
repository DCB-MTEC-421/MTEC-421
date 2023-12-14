using System.Collections;   // IEnumerator
using UnityEngine;

public class RhythmBehavior : MonoBehaviour {

    // Bundle all modification parameters together
    [System.Serializable]
    public class ModificationClass {
        [Range(0.0f, 1.0f)] public float X = 0.079f;
        [Range(0.0f, 1.0f)] public float Y = 0.044f;
        [Range(0.0f, 1.0f)] public float Z = 0.041f;
        [Range(0.0f, 1.0f)] public float Randomness = 0.079f;

        [HideInInspector] public Vector3 ScaleOriginal;
        [HideInInspector] public Vector3 ScaleAmount = Vector3.zero;
    }

    [SerializeField] ModificationClass _modifications;
    IEnumerator _actionProgress;

    [Range(0.0f, 50.0f)]
    [SerializeField] float _animationSpeed = 22.7f;

    void OnEnable() {
        // Retrieve original scale of GameObject
        _modifications.ScaleOriginal = transform.localScale;

        GameBehavior.OnMusicAction += PushAction;
    }

    void OnDisable() {
        // Unsubscribe
        GameBehavior.OnMusicAction -= PushAction;
    }

    void PushAction() {
        // Reset actionProgress coroutine and scale
        if (_actionProgress != null) {
            // Coroutine reset
            StopCoroutine(_actionProgress);
            _actionProgress = null;

            // Scale reset
            transform.localScale = _modifications.ScaleOriginal;
        }

        // Assign and start coroutine
        _actionProgress = ScalePerformer();
        StartCoroutine(_actionProgress);
    }

    IEnumerator ScalePerformer() {
        // Retrieve randomness to avoid writing too much
        float rand = _modifications.Randomness;

        // Modification for this iteration
        Vector3 scaleModifications = new(
            _modifications.X + Random.Range(0, rand),
            _modifications.Y + Random.Range(0, rand),
            _modifications.Z + Random.Range(0, rand)
        );

        // Create destination scaling based on original scaling with the above modifications applied
        _modifications.ScaleAmount = scaleModifications + _modifications.ScaleOriginal;

        bool isModified = false;

        while (!isModified) {
            bool scaleUpdated = UpdateScale(_modifications.ScaleAmount);

            if (scaleUpdated)
                isModified = true;

            yield return new WaitForEndOfFrame();
        }

        while (isModified) {
            bool scaleUpdated = UpdateScale(_modifications.ScaleOriginal);

            if (scaleUpdated)
                isModified = false;

            yield return new WaitForEndOfFrame();
        }
    }

    bool UpdateScale(Vector3 destinationScale) {
        // Linear interpolation to transition from an initial amount to a destination amount
        transform.localScale = Vector3.Lerp(
            transform.localScale,               // Initial amount
            destinationScale,                   // Final amount
            _animationSpeed * Time.deltaTime    // Progress coefficient
        );

        float distance = Vector3.Distance(transform.localScale, destinationScale);

        return distance <= 0.01f;
    }
}
