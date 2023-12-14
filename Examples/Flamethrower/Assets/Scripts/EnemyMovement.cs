using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
[RequireComponent(typeof(NavMeshAgent))]
// [RequireComponent(typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    // Contains data describing the triangulation of a NavMesh.
    public NavMeshTriangulation Triangulation;
    NavMeshAgent _agent;
    // Animator _animator;

    [SerializeField] [Range(0.0f, 3.0f)] float _waitDelay = 1.0f;

    void Awake() {
        _agent = GetComponent<NavMeshAgent>();
        // _animator = GetComponent<Animator>();
    }

    void Start() {
        GoToRandomPoint();
    }

    void Update() {
        float speed = _agent.velocity.magnitude;

        // _animator.SetFloat("Locomotion", Mathf.Clamp01(speed));
        // if (_agent.speed < 1)
        //     _animator.SetFloat("Idle", 1 - _agent.speed);
        // else
        //     _animator.SetFloat("Idle", 0);
    }

    public void GoToRandomPoint() {
        StartCoroutine(DoMoveToRandomPoint());
    }

    public void GoTowardsPlayer(Transform player) {
        StartCoroutine(DoMoveToPlayer(player));
    }

    public void StopMoving() {
        // _animator.SetFloat("Locomotion", 0);
        
        _agent.isStopped = true;
        StopAllCoroutines();
    }

    IEnumerator DoMoveToRandomPoint() {
        _agent.enabled = true;
        _agent.isStopped = false;

        WaitForSeconds wait = new(_waitDelay);

        while (true) {
            int index = Random.Range(1, Triangulation.vertices.Length - 1);
            _agent.SetDestination(Vector3.Lerp(
                Triangulation.vertices[index],
                Triangulation.vertices[index + (Random.value > 0.5 ? -1 : 1)],
                Random.value
            ));

            yield return null;
            yield return new WaitUntil(() => _agent.remainingDistance <= _agent.stoppingDistance);
            yield return wait;
        }
    }

    IEnumerator DoMoveToPlayer(Transform player) {
        _agent.enabled = true;
        _agent.isStopped = false;

        while (true) {
            _agent.SetDestination(player.position);

            yield return new WaitUntil(() =>
                Mathf.Approximately((_agent.destination - player.position).sqrMagnitude, 0)
            );
        }
    }
}
